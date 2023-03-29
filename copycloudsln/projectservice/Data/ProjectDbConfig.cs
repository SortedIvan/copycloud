using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using MongoDB.Driver;
using projectservice.Dto;
using projectservice.Models;
using System;
using System.Diagnostics;

namespace projectservice.Data
{
    public class ProjectDbConfig : IProjectDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<ProjectModel> projects;
        private readonly IMongoCollection<ProjectContentModel> projectContent;
        private readonly IMongoCollection<ProjectInviteModel> projectInvites;

        public ProjectDbConfig(IConfiguration _config, IMongoClient mongoClient)
        {
            this.config = _config;
            var db = mongoClient.GetDatabase(config.GetSection("ProjectDbSettings:DatabaseName").Value);

            // Db settings
            projects = db.GetCollection<ProjectModel>(config.GetSection("UserDbSettings:ProjectDbCollection").Value);
            projectContent = db.GetCollection<ProjectContentModel>(config.GetSection("UserDbSettings:ProjectContents").Value);
            projectInvites = db.GetCollection<ProjectInviteModel>(config.GetSection("UserDbSettings:ProjectContentDbCollection").Value);
        }

        public IMongoCollection<ProjectModel> GetProjectCollection()
        {
            return this.projects;
        }

        public IMongoCollection<ProjectInviteModel> GetProjectInvites()
        {
            return this.projectInvites;
        }

        public IMongoCollection<ProjectContentModel> GetProjectContent()
        {
            return this.projectContent;
        }

        public async Task<List<ProjectModel>> GetAllProjectsByCreator(string userEmail)
        {
            List<ProjectModel> projects = await this.projects.FindAsync(x => x.ProjectCreator == userEmail).Result.ToListAsync();
            return projects;
        }

        public async Task<ProjectModel> GetProjectByCreator(string userEmail, string projectId)
        {
            try
            {
                ProjectModel project = await this.projects.Find(x => x.ProjectCreator == userEmail && x.Id == projectId).FirstOrDefaultAsync();
                return project;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<ProjectModel>> GetAllJoinedProjects(string userEmail)
        {
            List<ProjectModel> projects = new List<ProjectModel>();
            projects = await this.projects.Find(x => x.ProjectUsers.Contains(userEmail)).ToListAsync();

            return projects;
        }

        public async Task<bool> AddUserToProject(string projectId, string userEmail)
        {
            var project = await projects.Find(x => x.Id == projectId).FirstOrDefaultAsync();
            if (project == null) { return false; }

            var filter = Builders<ProjectModel>.Filter.Eq(s => s.Id, projectId);
            var update = Builders<ProjectModel>.Update.AddToSet(x => x.ProjectUsers, userEmail);
            await projects.UpdateOneAsync(filter, update);

            return true;
        }

        public async Task<ProjectInviteModel> GetProjectInviteById(string inviteId)
        {
            try
            {
                ProjectInviteModel invite = await projectInvites.Find(x => x.Id == inviteId).FirstOrDefaultAsync();
                return invite;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ProjectInviteModel> GetProjectInviteBySenderInvitee(string invitee, string sender)
        {
            ProjectInviteModel inviteModel = await projectInvites.Find(x => x.Invitee == invitee && x.Sender == sender).FirstOrDefaultAsync();

            if (inviteModel == null)
            {
                return null;
            }

            return inviteModel;
        }

        public async Task<bool> DeleteProjectInvitation(string inviteId)
        {
            try
            {
                await projectInvites.DeleteOneAsync(x => x.Id == inviteId);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckInvitationExists(ProjectInvitationDto inviteDto)
        {
            try
            {
                var exists = await projectInvites.FindAsync(x => x.Invitee == inviteDto.Invitee && x.Sender == inviteDto.Sender).Result.ToListAsync();
                if (exists.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                return true;
            }
        }

        public async Task<bool> CreateProjectInvitation(ProjectInvitationDto inviteDto)
        {
            try
            {
                await this.projectInvites.InsertOneAsync(
                    new ProjectInviteModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Invitee = inviteDto.Invitee,
                        ProjectId= inviteDto.ProjectId,
                        Sender = inviteDto.Sender,
                        Secret= inviteDto.Secret
                    });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddNewProject(ProjectDto projectDto)
        {
            try
            {
                await this.projects.InsertOneAsync(
                    new ProjectModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectCreator = projectDto.ProjectCreator, // Id or email?
                        ProjectDescription= projectDto.ProjectDescription,
                        ProjectName= projectDto.ProjectName,
                        ProjectUsers = new List<string> { projectDto.ProjectCreator },// Add the creator to project users
                        
                    });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }
}
