namespace userservice.Models
{
    public class ProjectEvent
    {
        public string EventType { get; set; } = string.Empty;
        public DeleteUserData? DeleteUserData { get; set; }

    }

    public class DeleteUserData
    {
        public string UserEmail { get; set; } = string.Empty;
    }
    
}
