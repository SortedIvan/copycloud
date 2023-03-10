using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace authservice.Controllers
{
    public class AuthControlelr : ControllerBase
    {
        [Authorize]
        [HttpGet("api/auth/authenticate_cookie")]
        public async Task<bool> SetHttpOnlyCookie(string token)
        {
            HttpContext.Response.Cookies.Append("token", token,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
            return true;
        }

        [Authorize]
        [HttpGet("api/auth/test")]
        public async Task<bool> test()
        {
            return true;
        }


    }
}
