﻿using Azure.Storage.Blobs;
using projectservice.Data;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Utility;

namespace projectservice.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectDbConfig projectDb;
        private readonly IConfiguration config;
        private readonly IDocumentService documentService;
        private readonly IBlobStorageHelper blobStorage;

        public ProjectService(IProjectDbConfig _dbConfig, IConfiguration _config, IDocumentService _documentService, IBlobStorageHelper _blobStorage)
        {
            projectDb = _dbConfig;
            config = _config;
            this.documentService = _documentService;
            this.blobStorage = _blobStorage;
        }
        
        public async Task<Tuple<bool, string>> CreateProject(ProjectDto dto)
        {
            Tuple<bool, string> result = await projectDb.AddNewProject(dto);
            if (result.Item1)
            {
                await documentService.CreateDocument(result.Item2); // Send the projectid to the document service
                return Tuple.Create(true, result.Item2);
            }
            return Tuple.Create(false, "");
        } 

        public async Task<Tuple<bool, string>> DeleteProject(string projectId, string userRequesting)
        {
            if (await this.projectDb.CheckProjectExists(projectId))
            {
                ProjectModel project = await this.projectDb.GetProjectByProjectId(projectId);
                
                if (project.ProjectCreator != userRequesting)
                {
                    return Tuple.Create(false, "User is not the creator of the project" + project.ProjectCreator);
                }

                bool projectDeletedSuccesfully = await this.projectDb.DeleteProjectByProjectId(projectId);

                try
                {
                    BlobClient blob = await this.blobStorage.GetBlobClient(projectId);
                    await blob.DeleteAsync();
                    return Tuple.Create(projectDeletedSuccesfully, "Project deleted succesfully");
                }
                catch (Exception ex)
                {
                    return Tuple.Create(false, ex.Message);
                }

                return Tuple.Create(true, "Blob deleted succesfully");
            }
            return Tuple.Create(false, "Project does not exist");
        }

        public async Task<bool> DeleteUserFromProject(string userEmail)
        {
            try
            {

                List<ProjectModel> projects = await projectDb.GetAllJoinedProjects(userEmail);

                for (int i = 0; i < projects.Count; i++)
                {
                    if (projects[i].ProjectCreator == userEmail)
                    {
                        // Delete the entire project if the owner is deleted
                        await projectDb.DeleteProjectByProjectId(projects[i].Id); 
                    }
                    else
                    {
                        // Remove from project
                        await projectDb.DeleteUserFromProject(projects[i].Id, userEmail);
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
