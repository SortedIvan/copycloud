namespace emailappservice.Utility
{
    public static class EmailHtmlTemplateCreator
    {
        public static string GetProjectInviteHtmlTemplate(string projectInviteId, string projectName, string emailFrom, string token)
        {
            string template = File.ReadAllText(@"C:\Users\ISSD\Desktop\copycloudrepo\copycloudsln\emailappservice\Utility\index.html");
            template = template.Replace("#user#", emailFrom);
            template = template.Replace("#date#", DateTime.Now.ToString());
            template = template.Replace("#project#", projectName + " | " + projectInviteId);
            template = template.Replace("#href#", $"http://localhost:5127/api/acceptinvite?token={token}");
            return template;
        }

    }
}
