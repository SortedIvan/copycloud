using MongoDB.Driver;
using projectservice.Dto;
using projectservice.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace projectservice.Data
{
    public class ProjectDbConfig : IProjectDbConfig
    {
        private readonly IConfiguration config;
        private readonly IMongoCollection<ProjectModel> projects;
        private readonly IMongoCollection<ProjectInviteModel> projectInvites;
        public ProjectDbConfig(IConfiguration _config, IMongoClient mongoClient)
        {
            this.config = _config;
            var db = mongoClient.GetDatabase(config.GetSection("ProjectDbSettings:DatabaseName").Value);

            // Db settings
            projects = db.GetCollection<ProjectModel>(config.GetSection("ProjectDbSettings:ProjectDbCollection").Value);
            projectInvites = db.GetCollection<ProjectInviteModel>(config.GetSection("ProjectDbSettings:ProjectInviteDbCollection").Value);

        }

        public IMongoCollection<ProjectModel> GetProjectCollection()
        {
            return this.projects;
        }

        public IMongoCollection<ProjectInviteModel> GetProjectInvites()
        {
            return this.projectInvites;
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
            var filter = Builders<ProjectInviteModel>.Filter.And(
                Builders<ProjectInviteModel>.Filter.Where(p => p.Invitee == invitee),
                Builders<ProjectInviteModel>.Filter.Where(p => p.Sender == sender)
            );

            ProjectInviteModel inviteModel = await projectInvites.Find(filter).FirstOrDefaultAsync();

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

        public async Task<bool> CreateProjectInvitation(ProjectInvitationDto inviteDto, string secret)
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
                        Secret= secret
                    });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Tuple<bool, string>> AddNewProject(ProjectDto projectDto)
        {
            try
            {
                ProjectModel projectModel = new ProjectModel
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectCreator = projectDto.ProjectCreator, // Id or email?
                    ProjectDescription = projectDto.ProjectDescription,
                    ProjectName = projectDto.ProjectName,
                    ProjectUsers = new List<string> { projectDto.ProjectCreator },// Add the creator to project users

                };
                await this.projects.InsertOneAsync(projectModel);

                return Tuple.Create(true, projectModel.Id);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Null");
            }
        }

        public async Task<List<string>> GetAllUsersInProject(string projectId)
        {
            try
            {
                ProjectModel project = await this.projects.Find(x => x.Id == projectId).FirstOrDefaultAsync();
                return project.ProjectUsers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> CheckProjectExists(string projectId)
        {
            try
            {
                var exists = await projects.FindAsync(x => x.Id == projectId).Result.ToListAsync();
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

        public async Task<bool> CheckUserInProject(string userEmail, string projectId)
        {
            try
            {
                ProjectModel project = await projects.Find(x => x.Id == projectId).FirstOrDefaultAsync();
                for (int i = 0; i < project.ProjectUsers.Count; i++)
                {
                    if (project.ProjectUsers[i] == userEmail)
                    {
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<ProjectModel> GetProjectByProjectId(string projectId)
        {
            try
            {
                ProjectModel project = await projects.Find(x => x.Id == projectId).FirstOrDefaultAsync();
                return project;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
