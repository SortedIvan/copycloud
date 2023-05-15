using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
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
        private readonly EventHubProducerClient producer;
        public ProjectInviteService(IProjectDbConfig _dbConfig, IConfiguration _config)
        {
            projectDb = _dbConfig;
            config = _config;
            producer = new EventHubProducerClient(config.GetSection("EventHubConfig:ConnectionString").Value, config.GetSection("EventHubConfig:Hub").Value);
        }

        public async Task<string> CreateProjectInvite(ProjectInvitationDto inviteDto)
        {
            try
            {
                Tuple<string, string> tokenBaseSecretPair = InvitationTokenUtil.CreateInvitationForLink(inviteDto.ProjectId, inviteDto.Sender);

                bool success = await projectDb.CreateProjectInvitationForLink(inviteDto, tokenBaseSecretPair.Item2);

                if (success)
                {
                    return InvitationTokenUtil.Base64Encode(tokenBaseSecretPair.Item1);
                }

                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "";
            }
        }
        //{sender}:{projectId}:{secret}
        public async Task<Tuple<bool, string>> ConsumeInviteLink(string token, string email)
        {
            string baseDecodedToken = InvitationTokenUtil.Base64Decode(token);
            Tuple<string, string, string> result = InvitationTokenUtil.ParseInviteLink(baseDecodedToken);

            string sender = result.Item1;
            string projectId = result.Item2;
            string secret = result.Item3;

            //ProjectInvitationLink link = await projectDb.GetProjectInvitationLink(projectId, secret);

            bool inviteValid = await projectDb.CheckProjectInviteLinkExists(projectId, secret);

            if (!inviteValid)
            {
                return Tuple.Create(false, "Error with link");
            }
            
            await projectDb.AddUserToProject(projectId, email);

            return Tuple.Create(true, projectId);
        }

        public async Task<bool> SendInvite(ProjectInvitationDto inviteDto)
        {
            // Here send a message to the Email service
            // Must check if user is already in the project
            Tuple<string, string> tokenBaseSecretPair = InvitationTokenUtil.CreateInvitationToken(inviteDto.Invitee, inviteDto.ProjectId, inviteDto.Sender);
            bool success = await projectDb.CreateProjectInvitation(inviteDto, tokenBaseSecretPair.Item2);

            EmailMessage projectInviteMessage = new EmailMessage
            {
                Type = "invite",
                ProjectId = inviteDto.ProjectId,
                ProjectName = inviteDto.ProjectId,
                Receiver = inviteDto.Invitee,
                Sender = inviteDto.Sender,
                Token = InvitationTokenUtil.Base64Encode(tokenBaseSecretPair.Item1)
            };

            string messageBody = JsonConvert.SerializeObject(projectInviteMessage);
            // <------------------------- Event producing ------------------------------>
            List<EventData> emailEvent = new List<EventData>
            {
                new EventData(messageBody)
            };

            await producer.SendAsync(emailEvent); // Email is sent as event to hub

            
            
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

            EmailMessage projectInviteMessage = new EmailMessage
            {
                Type = "inviteAccept",
                ProjectId = tokenContents.Item3,
                Receiver = tokenContents.Item2,
                Sender = tokenContents.Item1,
            };

            string messageBody = JsonConvert.SerializeObject(projectInviteMessage);
            // <------------------------- Event producing ------------------------------>
            List<EventData> emailEvent = new List<EventData>
            {
                new EventData(messageBody)
            };

            await producer.SendAsync(emailEvent);

            return Tuple.Create(true, tokenContents.Item3);

        }
    }
}
