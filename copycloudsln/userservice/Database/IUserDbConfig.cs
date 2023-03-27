using MongoDB.Driver;
using userservice.Dto;
using userservice.Models;

namespace userservice.Database
{
    public interface IUserDbConfig
    {
        IMongoCollection<UserModel> GetUserCollection();
        Task<UserModel> GetUserByEmail(string userEmail);
        Task<bool> SaveUserDb(UserDtoRegister userDto, string firebaseId);
    }
}
