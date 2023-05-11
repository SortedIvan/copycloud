using MongoDB.Bson.Serialization.Attributes;

namespace projectservice.Models
{
    public class ProjectInvitationLink
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        [BsonElement("secret")]
        public string Secret { get; set; } = string.Empty;
        [BsonElement("invitee")]
        public string Sender { get; set; } = string.Empty;
        [BsonElement("projectid")]
        public string ProjectId { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
    }
}
