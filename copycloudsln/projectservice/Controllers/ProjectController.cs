using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projectservice.Data;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Services;
using System.Security.Claims;

namespace projectservice.Controllers
{
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly IProjectDbConfig projectDb;
        public ProjectController(IProjectService _projectService, IProjectDbConfig _projectDb)
        { 
            this.projectService = _projectService;
            this.projectDb = _projectDb;
        }


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

        [Authorize(Roles = "User")]
        [HttpGet("/api/getallprojects")]
        public async Task<ActionResult<List<ProjectModel>>> GetAllProjects()
        {
            var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();
            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            if (reqUserId == null)
            {
                return BadRequest("User not logged in or does not exist" + reqUserEmail + "is the email");
            }

            string userId = reqUserId.Value;
            string userEmail = reqUserEmail.Value;

            List<ProjectModel> userProjects = await projectDb.GetAllJoinedProjects(userEmail);

            return Ok(userProjects);
        }

        [Authorize(Roles ="User")]
        [HttpPost("/api/checkuserinproject")]
        public async Task<bool> CheckUserInProject(string projectId)
        {
            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            if (reqUserEmail == null)
            {
                return false;
            }

            string userEmail = reqUserEmail.Value;
            bool inProject = await projectDb.CheckUserInProject(userEmail, projectId);

            return inProject;

        }

        [HttpPost("/api/checkprojectexists")]
        public async Task<bool> CheckProjectExists(string projectId)
        {
            return await projectDb.CheckProjectExists(projectId);
        }

        [Authorize(Roles ="User")]
        [HttpGet("/api/auth/authorizationtestproject")]
        public async Task<string> TestProjectAuth()
        {
            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            string email = reqUserEmail.Value;
            return email;
        }
    }
}
