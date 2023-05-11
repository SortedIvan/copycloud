namespace projectservice.Dto
{
    public class ProjectDtoTemplate
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public string ProjectCreator { get; set; } = string.Empty; // The id of the user who created the project
        public IFormFile Template { get; set; }
    }
}
