

using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userservice.Services;
using userservice.Utils;

namespace userservice.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private ILogger<UserController> logger;
        public UserController(IUserService _userService, ILogger<UserController> logger)
        {
            this.userService = _userService;
            this.logger = logger;
        }

        //[Authorize(Roles ="User")]
        [HttpGet("/api/getcurrentuser")]
        public async Task<ActionResult<string>> GetCurrentUser()
        {
            var req = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            string userEmail = req.Value;

            if (userEmail== "")
            {
                return BadRequest("User does not exist");
            }

            return Ok(userEmail);
        }

        [Authorize(Roles = "User")]
        [HttpPost("/api/deleteself")]
        public async Task<IActionResult> DeleteSelf()
        {
            var req = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "email").FirstOrDefault();
            string userEmail = req.Value;

            if (userEmail == "")
            {
                return BadRequest("User not logged in or does not exist");
            }

            var cookie = Request.Headers.Cookie;
            string token = cookie.ElementAt(0);
            token = TokenParser.ParseToken(token);

            Tuple<bool, string> result = await userService.DeleteUser(userEmail, token);

            if (result.Item1)
            {
                return Ok("User has been deleted succesfully");
            }

            return BadRequest(result.Item2);
        }

    }
}
