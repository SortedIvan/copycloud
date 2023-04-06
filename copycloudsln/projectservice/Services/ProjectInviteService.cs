using Azure.Messaging.ServiceBus;
using EmailServiceMessages;
using Newtonsoft.Json;
using projectservice.Data;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Utility;
using System.Diagnostics;

namespace projectservice.Services
{
    public class ProjectInviteService : IProjectInviteService
    {
        // Invite person to project flow
        // 1) Create a token that has the structure {inviteeemail}:{projectinvitedto}:{invitor}:{secret}
        // 2) Send message to email service with the token
        // 3) User gets email and clicks on the url which is .../api/verifytoken/{token}
        // 4) Token is received and parsed
        // 5) Access token is checked (also contains the email of the invitee) and is compared and checked whether
        //    the user who accepted the token has the email inside his access token
        // 6) Project service finally takes the {projectinvitedto} and {invitor} and checks whether the invitor owns that project (or is already a user)
        // 7) Finally project service adds the user to the project

        private readonly IProjectDbConfig projectDb;
        private readonly IConfiguration config;
        private readonly ServiceBusSender serviceBusSender;
        private readonly ServiceBusClient busClient;
        public ProjectInviteService(IProjectDbConfig _dbConfig, IConfiguration _config)
        {
            projectDb = _dbConfig;
            config = _config;
            busClient = new ServiceBusClient(config.GetSection("ServiceBusConfig:ConnectionString").Value);
            serviceBusSender = busClient.CreateSender("projectemailqueue");
        }


        public async Task<bool> SendInvite(ProjectInvitationDto inviteDto)
        {
            // Here send a message to the Email service
            Tuple<string, string> tokenBaseSecretPair = InvitationTokenUtil.CreateInvitationToken(inviteDto.Invitee, inviteDto.ProjectId, inviteDto.Sender);
            bool success = await projectDb.CreateProjectInvitation(inviteDto, tokenBaseSecretPair.Item2);

            ProjectInviteMessage projectInviteMessage = new ProjectInviteMessage
            {
                ProjectId = inviteDto.ProjectId,
                ProjectName = inviteDto.ProjectId,
                Receiver = inviteDto.Invitee,
                Sender = inviteDto.Sender,
                Token = InvitationTokenUtil.Base64Encode(tokenBaseSecretPair.Item1)
            };

            string messageBody = JsonConvert.SerializeObject(projectInviteMessage);

            await serviceBusSender.SendMessageAsync(new ServiceBusMessage(messageBody)); // Message is sent to queue
            // SEND MESSAGE
            return true;
        }

        public async Task<Tuple<bool, string>> ConsumeInvite(string token)
        {
            string baseDecodedToken = InvitationTokenUtil.Base64Decode(token);

            Tuple<string, string, string, string> tokenContents = InvitationTokenUtil.ParseInviteToken(baseDecodedToken);

            ProjectInviteModel invite = await projectDb.GetProjectInviteBySenderInvitee(tokenContents.Item1, tokenContents.Item2); // Item1 == inviteeEmail | Item3 == Invitor

            if (invite == null)
            {
                Debug.WriteLine("Invite is null");
                return Tuple.Create(false, "Invite is null");
            }

            // Check whether the secret matches, validate the token
            if (invite.Secret != tokenContents.Item4) // Item4 == secret
            {
                Debug.WriteLine("Secret is incorrect");
                return Tuple.Create(false, "Secret is incorrect");
            }

            // Finally, accept the invite and add the user to the project
            await projectDb.DeleteProjectInvitation(invite.Id);

            await projectDb.AddUserToProject(tokenContents.Item3, tokenContents.Item1); // Item2 == ProjectInvitedToId

            // Send an email to the invitor that the user has been added

            return Tuple.Create(true, "Success");

        }
    }
}
