using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Services;

namespace projectservice.Controllers
{
    [ApiController]
    public class ProjectInvitationController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly IProjectInviteService projectInviteService;
        public ProjectInvitationController(IProjectService _projectService, IProjectInviteService _projectInviteService)
        { 
            this.projectService = _projectService;
            this.projectInviteService = _projectInviteService;
        }


        [HttpPost("/api/invitetoproject")]
        public async Task<IActionResult> InviteToProject(ProjectInvitationDto inviteDto)
        {
            await this.projectInviteService.SendInvite(inviteDto);
            return Ok("Invite succesful");
        }

        [HttpPost("/api/acceptinvite/")]
        public async Task<IActionResult> AcceptProjectInvite([FromQuery] string token)
        {
            // TODO: Check if the user is an actual user of the platform
            // Confirm his identity

            Tuple<bool, string> result = await this.projectInviteService.ConsumeInvite(token);
            return Ok(result);
        }
    }
}
