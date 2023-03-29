using MongoDB.Driver;
using projectservice.Models;
using System.Diagnostics;

namespace projectservice.Data
{
    public class ProjectDbConfig : IProjectDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<ProjectModel> projects;
        private readonly IMongoCollection<ProjectContentModel> projectContent;
        private readonly IMongoCollection<ProjectInviteModel> projectInvites;

        public ProjectDbConfig(IConfiguration _config, IMongoClient mongoClient)
        {
            this.config = _config;
            var db = mongoClient.GetDatabase(config.GetSection("UserDbSettings:DatabaseName").Value);

            // Db settings
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
