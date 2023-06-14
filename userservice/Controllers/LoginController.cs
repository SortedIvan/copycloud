using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using userservice.Database;
using userservice.Dto;
using userservice.Models;
using userservice.Services;

namespace userservice.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService loginService;
        private IUserDbConfig userDb;
        public LoginController(ILoginService _loginService, IUserDbConfig _userDb)
        {
            this.userDb = _userDb;
            loginService = _loginService;
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login(UserDtoLogin userDto)
        {
            if (userDto.Email == "")
            {
                return BadRequest("Please provide an email.");
            }

            if (userDto.Password == "")
            {
                return BadRequest("Please provide a valid password.");
            }

            FirebaseAuthLink firebaseAuth = await loginService.Login(userDto);

            if (!firebaseAuth.User.IsEmailVerified)
            {
                return BadRequest("Please verify your email before using the application.");
            }

            // Append the id token, short-lived
            HttpContext.Response.Cookies.Append("token", firebaseAuth.FirebaseToken.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true,
                    Secure = false,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

            // Append the access token, long-lived
            HttpContext.Response.Cookies.Append("rtoken", firebaseAuth.RefreshToken.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(120),
                    HttpOnly = true,
                    Secure = false,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,

                });

            return Ok("User logged in.");

        }

        [Authorize(Roles = "User")]
        [HttpGet("/api/auth/test")]
        public async Task<string> test()
        {
            var req = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();
            string reqValue = req.Value;
            return reqValue;
        }

        [HttpGet("/api/testgatewayuser")]
        public async Task<string> TestGateway()
        {
            return "Userservice gateway works";
        }
    }
}
