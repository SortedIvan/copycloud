namespace projectservice.Models
{
    public class ProjectContentModel
    {
        public string Id { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string AddedBy { get; set; } = string.Empty;
        public string CopyType { get ; set; } = string.Empty;    

        // Persistent data, save all of the invormation of the ContentModel here
        public object ContentData { get; set; } = new object();

    }
}
