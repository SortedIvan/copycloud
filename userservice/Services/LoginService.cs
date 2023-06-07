using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Refit;
using userservice.Dto;

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

        public async Task<FirebaseAuthLink> Login(UserDtoLogin loginDto)
        {

            // TODO: Exception handling
            FirebaseAuthLink firebaseAuthLink = await firebaseProvider.SignInWithEmailAndPasswordAsync(loginDto.Email, loginDto.Password);
            return firebaseAuthLink;
        }



    }
}
