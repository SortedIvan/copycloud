using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Refit;

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

            IAuthService authService = RestService.For<IAuthService>("https://localhost:5000/api/testcontroller");
            string secret_data = await authService.GetAuthService(firebaseAuthLink.FirebaseToken);

            return Ok(secret_data);
        }

        public interface IAuthService
        {
            [Get("/")]
            Task<string> GetAuthService([Authorize("Bearer")] string token);
        }
    }
}
