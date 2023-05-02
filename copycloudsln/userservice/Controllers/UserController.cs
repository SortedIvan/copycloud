

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace userservice.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        public UserController()
        {
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

    }
}
