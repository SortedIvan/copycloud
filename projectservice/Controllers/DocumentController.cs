using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projectservice.Data;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Services;
using projectservice.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace projectservice.Controllers
{
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IPusherHelper pusherHelper;
        private readonly IProjectService projectService;
        private readonly IProjectDbConfig projectDb;
        private readonly IDocumentService documentService;
        public DocumentController(IPusherHelper _pusherHelper, IProjectService _projectService, IProjectDbConfig _dbConfig, IDocumentService _documentService) 
        {
            this.pusherHelper = _pusherHelper;
            this.projectService = _projectService;
            this.projectDb = _dbConfig;
            this.documentService = _documentService;
        }

        [HttpPost("/api/authenticatepusher")]
        public async Task<IActionResult> AuthenticateToPusher()
        {
            var channel_name = HttpContext.Request.Form["channel_name"];
            var socket_id = HttpContext.Request.Form["socket_id"];
            string result = await pusherHelper.AuthenticatePusher(channel_name, socket_id, "123", "user");
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("/api/savedocument")]
        public async Task<IActionResult> SaveDocument(DocumentSaveDto save)
        {
            // Check whether the user exists

            var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();

            if (reqUserEmail == null)
            {
                return BadRequest("No such user exists. Please log in or refresh the page.");
            }

            string userEmail = reqUserEmail.Value;
            string userId = reqUserId.Value;

            //projectDb.Get

            if (!await projectDb.CheckProjectExists(save.ProjectId))
            {
                return BadRequest("Document does not exist. Please retry.");
            }

            Tuple<bool, string> result = await this.documentService.SaveDocument(save.Content, save.ProjectId);
            
            if (result.Item1)
            {
                bool resultUpdate = await projectDb.ChangeLastUpdated(save.ProjectId);
                return Ok(resultUpdate);
            }

            return BadRequest(result.Item2);

            // Check whether the project exist

        }

        [HttpGet("/api/getdocumentcontent")]
        public async Task<string> GetDocumentContent(string projectId)
        {
            return await documentService.GetDocumentContent(projectId);
        }



    }

}
