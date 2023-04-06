using MongoDB.Bson.Serialization.Attributes;

namespace contentservice.Models
{
    [BsonIgnoreExtraElements]
    public class CtoModel
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;

        [BsonElement("userid")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("copytype")]
        public string CopyType { get; set; } = string.Empty;

        [BsonElement("copy")]
        public string Copy { get; set; } = string.Empty;

        [BsonElement("copytone")]
        public string CopyTone { get; set; } = string.Empty;

        [BsonElement("copyaction")]
        public string CopyAction { get; set; } = string.Empty;

        [BsonElement("copycontext")]
        public string CopyContext { get; set; } = string.Empty;
    }
}
