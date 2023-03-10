using Firebase.Auth;
using Refit;
using System.Diagnostics;

namespace userservice.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IConfiguration config;
        private FirebaseAuthProvider firebaseProvider;

        public RegisterService(IConfiguration _config)
        {
            this.config = _config;
            firebaseProvider = new FirebaseAuthProvider(new FirebaseConfig(_config.GetSection("FirebaseSettings:firebase_api_key").Value));
        } 

        public async Task RegisterUser(string username, string password)
        {
            try
            {
                FirebaseAuthLink firebaseAuthLink = await firebaseProvider.CreateUserWithEmailAndPasswordAsync(username, password);
                // Here, send a message to the email service to confirm the user's email
                await firebaseProvider.SendEmailVerificationAsync(firebaseAuthLink);
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
