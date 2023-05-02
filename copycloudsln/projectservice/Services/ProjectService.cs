using projectservice.Data;
using projectservice.Dto;

namespace projectservice.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectDbConfig projectDb;
        private readonly IConfiguration config;
        private readonly IDocumentService documentService;

        public ProjectService(IProjectDbConfig _dbConfig, IConfiguration _config, IDocumentService _documentService)
        {
            projectDb = _dbConfig;
            config = _config;
            this.documentService = _documentService;
        }
        
        public async Task<bool> CreateProject(ProjectDto dto)
        {
            Tuple<bool, string> result = await projectDb.AddNewProject(dto);
            if (result.Item1)
            {
                await documentService.CreateDocument(result.Item2); // Send the projectid to the document service
                return true;
            }
            return false;
        } 
    }
}
