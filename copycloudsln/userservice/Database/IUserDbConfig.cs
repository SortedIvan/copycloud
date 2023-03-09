using MongoDB.Driver;
using userservice.Models;

namespace userservice.Database
{
    public interface IUserDbConfig
    {
        public IMongoCollection<User> GetUserCollection();
    }
}
