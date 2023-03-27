namespace userlibrary
{
    [BsonIgnoreExtraElements]
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("useremail")]
        public string UserEmail { get; set; } = string.Empty;

        [BsonElement("username")]
        public string UserName { get; set; } = string.Empty;
    }
}