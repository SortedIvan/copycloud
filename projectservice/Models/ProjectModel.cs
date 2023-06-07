using MongoDB.Bson.Serialization.Attributes;

namespace projectservice.Models
{
    [BsonIgnoreExtraElements]
    public class ProjectModel
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        [BsonElement("projectname")]
        public string ProjectName { get; set; } = string.Empty;
        [BsonElement("projectdescription")]
        public string ProjectDescription { get; set; } = string.Empty;
        [BsonElement("projectcreator")]
        public string ProjectCreator { get; set; } = string.Empty; // The email of the user who created the project
        [BsonElement("projectusers")]
        public List<string> ProjectUsers { get; set; } = new List<string>(); // The emails of joined users

        [BsonElement("projectcreationdate")]
        public DateTime CreationDate { get; set; }

        [BsonElement("projectlastupdated")]
        public DateTime LastUpdated { get; set; }


    }
}
