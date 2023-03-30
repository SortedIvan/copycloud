using projectservice.Data;
using projectservice.Dto;

namespace projectservice.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectDbConfig projectDb;
        private readonly IConfiguration config;

        public ProjectService(IProjectDbConfig _dbConfig, IConfiguration _config)
        {
            projectDb = _dbConfig;
            config = _config;
        }
        
        public async Task<bool> CreateProject(ProjectDto dto)
        {
            await projectDb.AddNewProject(dto);
            return true;
        } 
    }
}
