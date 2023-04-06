using ContentLibrary;
using MongoDB.Driver;
using projectservice.Data;

namespace projectservice.Services
{
    public class ProjectContentService : IProjectContentService
    {
        private readonly IProjectDbConfig dbConfig;
        public ProjectContentService(IProjectDbConfig _dbConfig)
        {
            this.dbConfig = _dbConfig;
        }

        public async Task<bool> AddContentToProject(object content, string contentId, string projectId, string addedById, string userEmail)
        {
            // Check whether the user who is adding content is a user in the project
            List<string> usersInProject = await dbConfig.GetAllUsersInProject(projectId);

            bool userPresent = false;
            for (int i = 0; i <usersInProject.Count; i++)
            {
                if (usersInProject[i].Equals(userEmail))
                {
                    userPresent = true;
                    break;
                }
            }
            
            if (!userPresent)
            {
                return false;
            }

            bool success = await dbConfig.AddContentToProject(content, contentId, projectId, addedById);
            return success;
        }
    }
}
