using Azure.Storage.Blobs.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projectservice.Data;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Services;
using projectservice.Utility;
using System.Security.Claims;

namespace projectservice.Controllers
{
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly IProjectDbConfig projectDb;
        private readonly IBlobStorageHelper blobHelper;
        public ProjectController(IProjectService _projectService, IProjectDbConfig _projectDb, IBlobStorageHelper _blobHelper)
        { 
            this.projectService = _projectService;
            this.projectDb = _projectDb;
            this.blobHelper = _blobHelper;
        }

        [Authorize(Roles = "User")]
        [HttpPost("/api/createproject")]
        public async Task<IActionResult> CreateNewProject(ProjectDto projectDto)
        {
            try
            {
                var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
                if (reqUserEmail == null)
                {
                    return BadRequest("User does not exist");
                }
                string projectCreator = reqUserEmail.Value;
                projectDto.ProjectCreator = projectCreator;

                Tuple<bool, string> result = await projectService.CreateProject(projectDto);

                if (!result.Item1) 
                {
                    return BadRequest("No such project");                
                }



                return Ok(result.Item2);
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

        [Authorize(Roles = "User")]
        [HttpGet("/api/getprojectbyid")]
        public async Task<ProjectModel> GetProjectById(string projectId)
        {
            var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();
            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            if (reqUserId == null)
            {
                return null;
            }

            string userId = reqUserId.Value;
            string userEmail = reqUserEmail.Value;

            ProjectModel project = await projectDb.GetProjectByProjectId(projectId);

            return project;

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

        [Authorize(Roles = "User")]
        [HttpPost("/api/deleteproject")]
        public async Task<IActionResult> DeleteProject(string projectId)
        {
            var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();
            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            if (reqUserId == null)
            {
                return BadRequest("User not logged in or does not exist" + reqUserEmail + "is the email");
            }

            string userId = reqUserId.Value;
            string userEmail = reqUserEmail.Value;

            Tuple<bool, string> result = await projectService.DeleteProject(projectId, userEmail);

            if (result.Item1)
            {
                return Ok("Project deleted succesfully.");
            }

            return BadRequest("There was an error: " + result.Item2 + userEmail);


        }





        [Authorize(Roles ="User")]
        [HttpGet("/api/auth/authorizationtestproject")]
        public async Task<string> TestProjectAuth()
        {
            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            string email = reqUserEmail.Value;
            return email;
        }

        [Authorize(Roles = "User")]
        [HttpPost("/api/createprojectwithtemplate")]
        public async Task<Tuple<bool, string>> CreateProjectWithTemplate([FromForm]ProjectDtoTemplate projectDto)
        {
            if (projectDto.ProjectName == null)
            {
                return new Tuple<bool, string>(false, "Please provide project name");
            }

            try
            {
                var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
                if (reqUserEmail == null)
                {
                    return new Tuple<bool, string>(false, "User is invalid or logged out.");
                }

                string projectCreator = reqUserEmail.Value;

                ProjectDto project = new ProjectDto
                {
                    ProjectCreator = projectCreator,
                    ProjectName = projectDto.ProjectName,
                    ProjectDescription = projectDto.ProjectDescription
                };

                Tuple<bool, string> result = await projectService.CreateProject(project);

                if (!result.Item1)
                {
                    return new Tuple<bool,string>(false,"No such project");
                }

                string templateContent = "";

                using (var reader = new StreamReader(projectDto.Template.OpenReadStream()))
                {
                    templateContent = await reader.ReadToEndAsync();
                }

                await blobHelper.UpdateDocumentContents(templateContent, result.Item2);

                return new Tuple<bool, string>(true, result.Item2);

            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false,ex.Message);
            }


        }
    }
}
