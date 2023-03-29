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

        public ProjectInviteService()
        {

        }

        public async Task<bool> AcceptProjectInvite(string inviteToken, string userEmail)
        {
            if (inviteToken == null)
            {
                return false;
            }

            if (userEmail == null)
            {
                return false;
            }



        }



    }
}
