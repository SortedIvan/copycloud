namespace projectservice.Models
{
    public class ProjectModel
    {
        public string Id { get; set; }  = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public string ProjectCreator { get; set; } = string.Empty; // The id of the user who created the project
        public string[] ProjectUsers { get; set; } = new string[0];
    }
}
