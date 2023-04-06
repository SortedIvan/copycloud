using contentservice.Dto;
using contentservice.Models;
using MongoDB.Driver;

namespace contentservice.Data
{
    public interface IContentDbConfig
    {
        IMongoCollection<CtoModel> GetUserCollection();
        Task<CtoModel> GetCtoCopyByUserId(string userId);
        Task<bool> SaveCtoCopy(CtoCopyDto copyDto, string userId);
        Task<List<CtoModel>> GetAllUserSavedCtoCopies(string userId);
    }
}