using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using projectservice.Dto;
using projectservice.Services;

namespace projectservice.Controllers
{
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        public ProjectController(IProjectService _projectService)
        { this.projectService = _projectService; }


        [HttpPost("/api/createproject")]
        public async Task<IActionResult> CreateNewProject(ProjectDto projectDto)
        {
            try
            {
                await projectService.CreateProject(projectDto);
                return Ok("Project created succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
