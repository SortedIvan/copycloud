using MongoDB.Bson.Serialization.Attributes;

namespace projectservice.Models
{
    [BsonIgnoreExtraElements]
    public class ProjectInviteModel
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        [BsonElement("secret")]
        public string Secret { get; set; } = string.Empty;
        [BsonElement("invitee")]
        public string Invitee { get; set; } = string.Empty;
        [BsonElement("sender")]
        public string Sender { get; set; } = string.Empty;
        [BsonElement("projectid")]
        public string ProjectId { get ; set; } = string.Empty;
    }
}
