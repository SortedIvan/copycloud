using Azure;
using Azure.Communication.Email;
using emailappservice.Service;
using emailappservice.Utility;
using Newtonsoft.Json.Linq;

namespace emailappservice.Service
{
    public class ProjectEmailService : IProjectEmailService
    {
        private readonly EmailClient emailClient;
        public ProjectEmailService()
        {
            this.emailClient = new EmailClient("endpoint=https://copycloudcommunication.communication.azure.com/;accesskey=Ob9foVVf9GUT6h5ZIcuDdk9dkVUhb+8Z6SpRDhZh5RTH4a8aMiv8w6dYqNZtGer9RpSKHKhhLuvCMPLElM74Xg==");
        }

        public async Task<bool> SendPersonJoinedEmail(string to, string who, string projectId, string projectName)
        {
            var emailSendOperation = await emailClient.SendAsync(
                wait: WaitUntil.Completed,
                from: "DoNotReply@0b9008ef-0c6c-43c0-9473-3f806f96e91a.azurecomm.net",
                to: to,
                subject: projectName,
                htmlContent: $"<h3> The user {who} has succesfully joined your project {projectName}|{projectId} </h3>");

            if (emailSendOperation.Value.Status.Equals(true))
            {
                return true;
            }

            return false;
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
