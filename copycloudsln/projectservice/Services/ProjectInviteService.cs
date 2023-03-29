using projectservice.Data;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Utility;

namespace projectservice.Services
{
    public class ProjectInviteService
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
        public ProjectInviteService(IProjectDbConfig _dbConfig, IConfiguration _config)
        {
            projectDb = _dbConfig;
            config = _config;
        }

        public async Task<bool> SendInvite(ProjectInvitationDto inviteDto)
        {
            // Here send a message to the Email service

            bool success = await projectDb.CreateProjectInvitation(inviteDto);

            // SEND MESSAGE

            return true;
        }

        public async Task<bool> ConsumeInvite(string token)
        {
            Tuple<string, string, string, string> tokenContents = InvitationTokenUtil.ParseInviteToken(token);

            ProjectInviteModel invite = await projectDb.GetProjectInviteBySenderInvitee(tokenContents.Item1, tokenContents.Item3); // Item1 == inviteeEmail | Item3 == Invitor

            if (invite == null)
            {
                return false;

            }

            // Check whether the secret matches, validate the token
            if (invite.Secret != tokenContents.Item4) // Item4 == secret
            {
                return false;
            }

            // Finally, accept the invite and add the user to the project
            await projectDb.DeleteProjectInvitation(invite.Id);

            await projectDb.AddUserToProject(tokenContents.Item2, tokenContents.Item1); // Item2 == ProjectInvitedToId

            // Send an email to the invitor that the user has been added

            return true;

        }
    }
}
