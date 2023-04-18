using Microsoft.AspNetCore.Mvc;
using projectservice.Utility;
using System.Security.Claims;

namespace projectservice.Controllers
{
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IPusherHelper pusherHelper;
        public DocumentController(IPusherHelper _pusherHelper) 
        {
            this.pusherHelper = _pusherHelper;
        }

        [HttpPost("/api/authenticatepusher")]
        public async Task<IActionResult> AuthenticateToPusher()
        {
            //var reqUserEmail = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            //var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "user_id").FirstOrDefault();

            //if (reqUserEmail == null)
            //{
            //    return BadRequest("No such user exists. Please log in or refresh the page.");
            //}
            
            //string userEmail = reqUserEmail.Value;
            //string userId = reqUserId.Value;

            var channel_name = HttpContext.Request.Form["channel_name"];
            var socket_id = HttpContext.Request.Form["socket_id"];
            //var channel_name = channel_name;
            //var socket_id = socket_id
            string result = await pusherHelper.AuthenticatePusher(channel_name, socket_id, "123", "user");
            return Ok(result);
        }

        [HttpPost("/api/savedocument")]
        public async Task<IActionResult> SaveDocument(string documentContent, string documentId)
        {
            // Check whether the user exists
            // Check whether the project exist
            
        }

    }

}
