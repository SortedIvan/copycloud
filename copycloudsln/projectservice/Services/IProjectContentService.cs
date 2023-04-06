using ContentLibrary;

namespace projectservice.Services
{
    public interface IProjectContentService
    {
        Task<bool> AddContentToProject(object ctoModel,string contentId, string projectId, string addedById, string userEmail);
        
    }
}
