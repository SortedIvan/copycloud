using ContentLibrary;
using MongoDB.Driver;
using projectservice.Dto;
using projectservice.Models;

namespace projectservice.Data
{
    public interface IProjectDbConfig
    {
        IMongoCollection<ProjectModel> GetProjectCollection();
        IMongoCollection<ProjectInviteModel> GetProjectInvites();
        IMongoCollection<ProjectContentModel> GetProjectContent();
        Task<List<ProjectModel>> GetAllProjectsByCreator(string userEmail);
        Task<ProjectModel> GetProjectByCreator(string userEmail, string projectId);
        Task<List<ProjectModel>> GetAllJoinedProjects(string userEmail);
        Task<bool> AddUserToProject(string projectId, string userEmail);
        Task<ProjectInviteModel> GetProjectInviteById(string inviteId);
        Task<ProjectInviteModel> GetProjectInviteBySenderInvitee(string invitee, string sender);
        Task<bool> DeleteProjectInvitation(string inviteId);
        Task<bool> CheckInvitationExists(ProjectInvitationDto inviteDto);
        Task<bool> CreateProjectInvitation(ProjectInvitationDto inviteDto, string secret);
        Task<bool> AddNewProject(ProjectDto projectDto);
        Task<bool> AddContentToProject(object content, string contentId, string projectId, string addedBy);
        Task<List<string>> GetAllUsersInProject(string projectId);
    }
}