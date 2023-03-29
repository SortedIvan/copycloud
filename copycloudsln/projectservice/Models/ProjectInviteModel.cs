﻿namespace projectservice.Models
{
    public class ProjectInviteModel
    {
        public string Id { get; set; } = string.Empty;
        public byte[] Secret { get; set; } = new byte[0];
        public string Invitee { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string ProjectId { get ; set; } = string.Empty;
    }
}
