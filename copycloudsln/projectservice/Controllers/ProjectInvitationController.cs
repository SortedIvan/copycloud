using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projectservice.Dto;
using projectservice.Services;
using projectservice.Utility;
using System.Security.Claims;

namespace projectservice.Controllers
{
    [ApiController]
    public class ProjectInvitationController : ControllerBase
    {
        private readonly IProjectInviteService projectInviteService;

        public ProjectInvitationController(IProjectInviteService _projectInviteService)
        { 
            this.projectInviteService = _projectInviteService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("/api/invitetoproject")]
        public async Task<IActionResult> InviteToProject(ProjectInvitationDto inviteDto)
        {
            await this.projectInviteService.SendInvite(inviteDto);
            return Ok("Invite succesful");
        }

        [Authorize(Roles = "User")]
        [HttpPost("/api/createprojectinvite")]
        public async Task<string> CreateProjectInvite(ProjectInvitationDto inviteDto)
        {
            string invite = await this.projectInviteService.CreateProjectInvite(inviteDto);
            return "http://localhost:5127/api/acceptinvitefromlink?token=" + invite;
        }

        [Authorize(Roles = "User")]
        [HttpGet("/api/acceptinvitefromlink")]
        public async Task<IActionResult> AcceptInviteFromLink([FromQuery]string token)
        {
            var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();
            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            if (reqUserId == null)
            {
                return BadRequest("User is not logged in");
            }

            string userId = reqUserId.Value;
            string userEmail = reqUserEmail.Value;

            Tuple<bool, string> result = await this.projectInviteService.ConsumeInviteLink(token, userEmail);

            if (result.Item1)
            {
                return Redirect($"http://localhost:8080/project/{result.Item2}");
            }
            return BadRequest("Something went wrong with the server");
        }

        [Authorize(Roles = "User")]
        [HttpGet("/api/acceptinvite/")]
        public async Task<IActionResult> AcceptProjectInvite([FromQuery] string token)
        {
            // TODO: Check if the user is an actual user of the platform
            // Confirm his identity

            Tuple<bool, string> result = await this.projectInviteService.ConsumeInvite(token);

            if (result.Item1)
            {
                return Redirect($"http://localhost:8080/project/{result.Item2}");
            }
            return BadRequest(result);
        }

        [HttpPost("/api/getencodedtoken/")]
        public async Task<IActionResult> GetToken(string token)
        {
            return Ok(InvitationTokenUtil.Base64Encode(token));
        }

        [HttpPost("/api/testtokendecoding/")]
        public async Task<IActionResult> DecodeToken(string token)
        {
            return Ok(InvitationTokenUtil.ParseInviteToken(InvitationTokenUtil.Base64Decode(token)));
        }


    }
}
