namespace emailappservice.Utility
{
    public static class EmailHtmlTemplateCreator
    {
        public static string GetProjectInviteHtmlTemplate(string projectInviteId, string projectName, string emailFrom, string token)
        {
            string template = File.ReadAllText(@"index.html");
            template = template.Replace("#user#", emailFrom);
            template = template.Replace("#date#", DateTime.Now.ToString());
            template = template.Replace("#project#", projectName + " | " + projectInviteId);
            template = template.Replace("#href#", $"https://copycloud.work/api/acceptinvite?token={token}");
            return template;
        }

    }
}
