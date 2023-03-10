using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Diagnostics;

namespace userservice.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("/api/login")]
        public async Task<IActionResult> Login(string username, string password)
        {

            var api_key = "AIzaSyCHIW7gTKmEBTS5EbO7-KAkk-q6p6rw0QU\r\n";
            FirebaseAuthProvider _provider = new FirebaseAuthProvider(new FirebaseConfig(api_key));

            FirebaseAuthLink firebaseAuthLink = await _provider.SignInWithEmailAndPasswordAsync(username, password);

            IAuthService authService = RestService.For<IAuthService>("https://localhost:5000/api/authenticate_cookie");

            bool result = await authService.GetAuthService(firebaseAuthLink.FirebaseToken);


            return Ok(result);
        }

        public interface IAuthService
        {
            [Get("/")]
            Task<bool> GetAuthService([Authorize("Bearer")] string token);
        }
    }
}
