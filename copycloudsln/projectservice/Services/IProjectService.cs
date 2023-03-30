using projectservice.Dto;

namespace projectservice.Services
{
    public interface IProjectService
    {
        Task<bool> CreateProject(ProjectDto dto);
    }
}
