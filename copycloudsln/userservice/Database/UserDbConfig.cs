using userservice.Models;
using MongoDB.Driver;

namespace userservice.Database
{
    public class UserDbConfig : IUserDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<User> users;

        public UserDbConfig(IConfiguration config, IMongoClient mongoClient)
        {
            this.config = config;
            var db = mongoClient.GetDatabase(config.GetSection("UserDbSettings:DatabaseName").Value);

            users = db.GetCollection<User>(config.GetSection("UserDbSettings:UserDbCollection").Value);
        }

        public IMongoCollection<User> GetUserCollection()
        {
            return this.users;
        }
    }
}