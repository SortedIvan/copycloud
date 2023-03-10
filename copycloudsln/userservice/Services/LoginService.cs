using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Refit;

namespace userservice.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration config;
        private FirebaseAuthProvider firebaseProvider;
        public LoginService(IConfiguration _config)
        {
            this.config = _config;
            firebaseProvider = new FirebaseAuthProvider(new FirebaseConfig(_config.GetSection("FirebaseSettings:firebase_api_key").Value));
        }

        public async Task<FirebaseAuthLink> Login(string username, string password)
        {
            // TODO: Exception handling
            FirebaseAuthLink firebaseAuthLink = await firebaseProvider.SignInWithEmailAndPasswordAsync(username, password);
            return firebaseAuthLink;
        }



    }
}
