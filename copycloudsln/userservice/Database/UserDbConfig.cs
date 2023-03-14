using userservice.Models;
using MongoDB.Driver;
using Amazon.Util;
using static System.Reflection.Metadata.BlobBuilder;

namespace userservice.Database
{
    public class UserDbConfig : IUserDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<User> users;

        public UserDbConfig(IConfiguration _config, IMongoClient mongoClient)
        {
            this.config = _config;
            var db = mongoClient.GetDatabase(config.GetSection("UserDbSettings:DatabaseName").Value);
            users = db.GetCollection<User>(config.GetSection("UserDbSettings:UserDbCollection").Value);
        }

        public IMongoCollection<User> GetUserCollection()
        {
            return this.users;
        }
    }
}

//private readonly IConfiguration config;
//private string LoggingCollectionName;
//private string DatabaseName;
//private readonly IMongoCollection<Log> _logs;

//public LoggingDbConfig(IConfiguration _config, IMongoClient mongoClient)
//{
//    this.config = _config;
//    this.LoggingCollectionName = config.GetSection("LogDbSettings:ExcelLogCollectionName").Value;
//    this.DatabaseName = config.GetSection("LogDbSettings:DatabaseName").Value;

//    var database = mongoClient.GetDatabase(DatabaseName);
//    _logs = database.GetCollection<Log>(LoggingCollectionName);
//}

//public IMongoCollection<Log> GetLogCollection()
//{
//    return _logs;
//}
