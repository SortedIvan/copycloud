using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Diagnostics;
using contentservice.Models;
using contentservice.Dto;
using contentservice.Data;

namespace userservice.Database
{
    public class UserDbConfig : IContentDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<CtoModel> ctoCopies;

        public UserDbConfig(IConfiguration _config, IMongoClient mongoClient)
        {
            this.config = _config;
            var db = mongoClient.GetDatabase(config.GetSection("ContentDbSettings:DatabaseName").Value);
            ctoCopies = db.GetCollection<CtoModel>(config.GetSection("ContentDbSettings:CtoCollection").Value);
        }

        public IMongoCollection<CtoModel> GetUserCollection()
        {
            return this.ctoCopies;
        }

        public async Task<CtoModel> GetCtoCopyByUserId(string userId)
        {
            try
            {
                CtoModel ctoCopy = await this.ctoCopies.Find(x => x.UserId == userId).FirstOrDefaultAsync();
                return ctoCopy;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> SaveCtoCopy(CtoCopyDto copyDto, string userId)
        {
            try
            {
                await this.ctoCopies.InsertOneAsync(
                    new CtoModel
                    {
                        Id = new Guid().ToString(),
                        UserId = userId,
                        Copy = copyDto.Copy,
                        CopyAction = copyDto.CopyAction,
                        CopyContext = copyDto.CopyContext,
                        CopyTone = copyDto.CopyTone
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
