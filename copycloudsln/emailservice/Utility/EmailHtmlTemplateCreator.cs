using System.IO;

namespace emailservice.Utility
{
    public static class EmailHtmlTemplateCreator
    {
        public static string GetProjectInviteHtmlTemplate(string projectInviteId, string projectName, string emailFrom, string token)
        {
            string template = File.ReadAllText(@"Utility/index.html");
            template = template.Replace("#user#", emailFrom);
            template = template.Replace("#date#", DateTime.Now.ToString());
            template = template.Replace("#project#", projectName + " | " + projectInviteId);
            template = template.Replace("#href#", $"http://localhost:6003/api/acceptinvite?token={token}");
            return template;
        }

    }
}
