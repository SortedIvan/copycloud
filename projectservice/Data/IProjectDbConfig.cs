using MongoDB.Driver;
using projectservice.Dto;
using projectservice.Models;

namespace projectservice.Data
{
    public interface IProjectDbConfig
    {
        IMongoCollection<ProjectModel> GetProjectCollection();
        IMongoCollection<ProjectInviteModel> GetProjectInvites();
        Task<List<ProjectModel>> GetAllProjectsByCreator(string userEmail);
        Task<ProjectModel> GetProjectByCreator(string userEmail, string projectId);
        Task<ProjectModel> GetProjectByProjectId(string projectId);
        Task<List<ProjectModel>> GetAllJoinedProjects(string userEmail);
        Task<bool> AddUserToProject(string projectId, string userEmail);
        Task<ProjectInviteModel> GetProjectInviteById(string inviteId);
        Task<ProjectInviteModel> GetProjectInviteBySenderInvitee(string invitee, string sender);
        Task<bool> DeleteProjectInvitation(string inviteId);
        Task<bool> CheckInvitationExists(ProjectInvitationDto inviteDto);
        Task<bool> CreateProjectInvitation(ProjectInvitationDto inviteDto, string secret);
        Task<Tuple<bool, string>> AddNewProject(ProjectDto projectDto);
        Task<List<string>> GetAllUsersInProject(string projectId);
        Task<bool> CheckProjectExists(string projectId);
        Task<bool> CheckUserInProject(string userEmail, string projectId);
        Task<bool> CreateProjectInvitationForLink(ProjectInvitationDto inviteDto, string secret);
        Task<ProjectInvitationLink> GetProjectInvitationLink(string projectId, string secret);
        Task<bool> CheckProjectInviteLinkExists(string projectId, string secret);
        Task<bool> DeleteProjectByProjectId(string projectId);
        Task<Tuple<bool, string>> DeleteUserFromProject(string projectId, string userEmail);
    }
}