using projectservice.Dto;

namespace projectservice.Services
{
    public interface IProjectService
    {
        Task<Tuple<bool, string>> CreateProject(ProjectDto dto);
        Task<Tuple<bool, string>> DeleteProject(string projectId, string userRequesting);
        Task<bool> DeleteUserFromProject(string userEmail);
    }
}
