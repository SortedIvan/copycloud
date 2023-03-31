namespace emailservice.Service
{
    public interface IProjectEmailService
    {
        Task<bool> SendPersonJoinedEmail(string to, string who, string projectId, string projectName);
        Task<bool> SendProjectInviteEmail(string receiver, string sender,string inviteToken, string projectName, string projectId);
    }
}
