using projectservice.Dto;

namespace projectservice.Services
{
    public interface IProjectInviteService
    {
        Task<Tuple<bool, string>> ConsumeInvite(string token);
        Task<bool> SendInvite(ProjectInvitationDto inviteDto);
        Task<string> CreateProjectInvite(ProjectInvitationDto inviteDto);
        Task<Tuple<bool, string>> ConsumeInviteLink(string token, string email);
    }
}
