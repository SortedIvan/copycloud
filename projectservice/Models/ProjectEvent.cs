namespace projectservice.Models
{
    public class ProjectEvent
    {
        public string EventType { get; set; } = string.Empty;
        public DeleteUserData? DeleteUserData { get; set; }

    }

    public class DeleteUserData
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;

        public string ProjectId { get; set; } = string.Empty;
    }
}
