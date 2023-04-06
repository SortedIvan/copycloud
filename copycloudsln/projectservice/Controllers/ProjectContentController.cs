using Microsoft.AspNetCore.Mvc;
using projectservice.Services;
using System.Security.Claims;

namespace projectservice.Controllers
{
    [ApiController]
    public class ProjectContentController : ControllerBase
    {
        private readonly IProjectContentService projectContentService;
        private readonly IProjectService projectService;
        public ProjectContentController(IProjectContentService _projectContentService)
        {
            this.projectContentService = _projectContentService;
        }

        [HttpPost("/api/addexistingcontent/")]
        public async Task<ActionResult> AddExistingContentToProject(object content, string contentId, string addedBy, string projectId)
        {
            if (content == null) 
            {
                return BadRequest("Content can't be empty");
            }

            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();

            if (reqUserEmail == null)
            {
                return BadRequest("No such copy or user exists. Please log in or refresh the page.");
            }

            string userEmail = reqUserEmail.Value;

            bool successs = await projectContentService.AddContentToProject(content, contentId, projectId, addedBy, userEmail);

            return Ok(successs);
        }
    }
}
