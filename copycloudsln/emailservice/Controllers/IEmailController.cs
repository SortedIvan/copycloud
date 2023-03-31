using emailservice.Service;
using Microsoft.AspNetCore.Mvc;

namespace emailservice.Controllers
{
    [ApiController]
    public class IEmailController : ControllerBase 
    {
        private IProjectEmailService projectEmailService;
        public IEmailController(IProjectEmailService _projectEmailService) 
        { 
            this.projectEmailService = _projectEmailService;
        }

        [HttpPost("/api/testemail")]
        public async Task<IActionResult> TestEmail(string receiver,string sender, string inviteToken, string projectName, string projectId)
        {
            bool success = await projectEmailService.SendProjectInviteEmail(receiver,sender, inviteToken, projectName, projectId);
            return Ok(success);
        }
    }
}
