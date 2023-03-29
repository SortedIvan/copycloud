namespace projectservice.Models
{
    public class ProjectInviteModel
    {
        public string Id { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string Invitee { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string ProjectId { get ; set; } = string.Empty;
    }
}
