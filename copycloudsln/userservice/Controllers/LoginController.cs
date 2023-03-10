using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userservice.Services;

namespace userservice.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService loginService;
        public LoginController(ILoginService _loginService)
        {
            loginService = _loginService;
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            FirebaseAuthLink firebaseAuth = await loginService.Login(username, password);

            // Append the id token, short-lived
            HttpContext.Response.Cookies.Append("token", firebaseAuth.FirebaseToken.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

            // Append the access token, long-lived
            HttpContext.Response.Cookies.Append("rtoken", firebaseAuth.RefreshToken.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    
                });

            return Ok("User logged in.");

        }

        [Authorize(Roles = "User")]
        [HttpGet("api/auth/test")]
        public async Task<bool> test()
        {
            return true;
        }

    }
}
