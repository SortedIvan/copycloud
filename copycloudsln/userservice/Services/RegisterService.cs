using Firebase.Auth;
using Refit;
using System.Diagnostics;

namespace userservice.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IConfiguration config;
        private string api_key;
        private FirebaseAuthProvider mongoProvider;

        public RegisterService(IConfiguration _config)
        {
            this.config = _config;
            api_key = "AIzaSyCHIW7gTKmEBTS5EbO7-KAkk-q6p6rw0QU\r\n";
            mongoProvider = new FirebaseAuthProvider(new FirebaseConfig(api_key));
        } 

        public async Task RegisterUser(string username, string password)
        {
            try
            {
                FirebaseAuthLink firebaseAuthLink = await mongoProvider.CreateUserWithEmailAndPasswordAsync(username, password);
                IAuthService authService = RestService.For<IAuthService>("http://localhost:5000/api/testcontroller");
                Debug.WriteLine(await authService.GetAuthService(firebaseAuthLink.FirebaseToken));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public interface IAuthService
        {
            [Get("/")]
            Task<string> GetAuthService([Authorize("Bearer")] string token);
        }

        private void CreateUserDb(string username, string password)
        {

        }
    }
}
