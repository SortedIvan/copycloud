using projectservice.Dto;

namespace projectservice.Services
{
    public interface IProjectService
    {
        Task<Tuple<bool, string>> CreateProject(ProjectDto dto);
    }
}
