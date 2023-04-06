using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Diagnostics;
using contentservice.Models;
using contentservice.Dto;
using contentservice.Data;

namespace userservice.Database
{
    public class ContentDbConfig : IContentDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<CtoModel> ctoCopies;
        //private readonly IMongoCollection<object> 

        public ContentDbConfig(IConfiguration _config, IMongoClient mongoClient)
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

        public async Task<List<CtoModel>> GetAllUserSavedCtoCopies(string userId)
        {
            try
            {
                List<CtoModel> userCopies = await this.ctoCopies.Find(x => x.UserId == userId).ToListAsync();
                return userCopies;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> SaveCtoCopy(CtoCopyDto copyDto, string userId)
        {
            await this.ctoCopies.InsertOneAsync(
                new CtoModel
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Copy = copyDto.Copy,
                    CopyAction = copyDto.CopyAction,
                    CopyContext = copyDto.CopyContext,
                    CopyTone = copyDto.CopyTone
                });
            return true;

        }


    }
}
