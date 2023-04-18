using userservice.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Diagnostics;
using userservice.Dto;

namespace userservice.Database
{
    public class UserDbConfig : IUserDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<UserModel> users;

        public UserDbConfig(IConfiguration _config, IMongoClient mongoClient)
        {
            this.config = _config;
            var db = mongoClient.GetDatabase(config.GetSection("UserDbSettings:DatabaseName").Value);
            users = db.GetCollection<UserModel>(config.GetSection("UserDbSettings:UserDbCollection").Value);
        }

        public IMongoCollection<UserModel> GetUserCollection()
        {
            return this.users;
        }

        public async Task<UserModel> GetUserByEmail(string userEmail)
        {
            try
            {
                UserModel user = await this.users.Find(x => x.UserEmail == userEmail).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> CheckUserExists(string userEmail)
        {
            List<UserModel> check = await this.users.Find(x => x.UserEmail == userEmail).ToListAsync();

            if (check.Count > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> SaveUserDb(UserDtoRegister userDto, string firebaseId)
        {
            try
            {
                await this.users.InsertOneAsync(
                    new Models.UserModel
                    {
                        Id = firebaseId,
                        UserEmail = userDto.Email,
                        UserName = userDto.Name
                    });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
