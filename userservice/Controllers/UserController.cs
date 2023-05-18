

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userservice.Services;

namespace userservice.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            this.userService = _userService;
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

            Tuple<bool, string> result = await userService.DeleteUser(userEmail);

            if (result.Item1)
            {
                return Ok("User has been deleted succesfully");
            }

            return BadRequest(result.Item2);
        }

    }
}
