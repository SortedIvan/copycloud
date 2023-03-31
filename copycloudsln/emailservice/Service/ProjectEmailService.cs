using Azure;
using Azure.Communication.Email;
using emailservice.Utility;

namespace emailservice.Service
{
    public class ProjectEmailService : IProjectEmailService
    {
        private readonly IConfiguration config;
        private readonly EmailClient emailClient;
        public ProjectEmailService(IConfiguration _config)
        {
            this.config = _config;
            this.emailClient = new EmailClient(config.GetSection("EmailConfig:ConnectionString").Value);
        }

        public async Task<bool> SendPersonJoinedEmail(string to, string who, string projectId, string projectName)
        {
            return true;
        }

        public async Task<bool> SendProjectInviteEmail(string receiver, string sender, string inviteToken, string projectName, string projectId)
        {
            var emailSendOperation = await emailClient.SendAsync(
                wait: WaitUntil.Completed,
                from: "DoNotReply@0b9008ef-0c6c-43c0-9473-3f806f96e91a.azurecomm.net",
                to: receiver,
                subject: projectName,
                htmlContent: EmailHtmlTemplateCreator.GetProjectInviteHtmlTemplate(projectId, projectName, sender, inviteToken));

            if (emailSendOperation.Value.Status.Equals(true))
            {
                return true;
            }

            return false;

        }
    }
}
