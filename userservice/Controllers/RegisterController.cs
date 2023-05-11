using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Refit;
using userservice.Dto;
using userservice.Services;

namespace userservice.Controllers
{
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService registerService;
        public RegisterController(IRegisterService _registerService)
        { this.registerService = _registerService; }


        [HttpPost("/api/register")]
        public async Task<ActionResult<Tuple<bool, string>>> Register(UserDtoRegister userDto)
        {   
            try
            {
                Tuple<bool, string> result = await registerService.RegisterUser(userDto);
                if (result.Item1)
                {
                    return Ok("Please verify your email to start using the application!");
                }
                return BadRequest(result.Item2);
            }
            catch (FirebaseAuthException ex)
            {
                switch (ex.Reason)
                {
                    case AuthErrorReason.EmailExists:
                        return BadRequest("A user with this email already exist. Forgotten password?");
                    case AuthErrorReason.WeakPassword:
                        return BadRequest("Password not strong enough. Please use more than 6 characters.");
                    case AuthErrorReason.MissingPassword:
                        return BadRequest("Password missing.");
                    case AuthErrorReason.MissingEmail:
                        return BadRequest("Email missing");
                }

                return BadRequest(ex.Message);
            }
        }
    }
}
