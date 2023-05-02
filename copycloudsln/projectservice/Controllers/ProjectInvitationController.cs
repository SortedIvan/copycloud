using Microsoft.AspNetCore.Mvc;
using projectservice.Dto;
using projectservice.Services;
using projectservice.Utility;


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


        [HttpPost("/api/invitetoproject")]
        public async Task<IActionResult> InviteToProject(ProjectInvitationDto inviteDto)
        {
            await this.projectInviteService.SendInvite(inviteDto);
            return Ok("Invite succesful");
        }

        [HttpGet("/api/acceptinvite/")]
        public async Task<IActionResult> AcceptProjectInvite([FromQuery] string token)
        {
            // TODO: Check if the user is an actual user of the platform
            // Confirm his identity

            Tuple<bool, string> result = await this.projectInviteService.ConsumeInvite(token);

            if (result.Item1)
            {
                return Redirect($"http://localhost:8080/app/myboard/{result.Item2}");
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
