using DnsClient;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Refit;
using System.Diagnostics;
using userservice.Database;
using userservice.Dto;
using userservice.Models;

namespace userservice.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IConfiguration config;
        private FirebaseAuthProvider firebaseProvider;
        private readonly IUserDbConfig userDbConfig;

        public RegisterService(IConfiguration _config, IUserDbConfig _userDbConfig)
        {
            this.config = _config;
            firebaseProvider = new FirebaseAuthProvider(new FirebaseConfig(_config.GetSection("FirebaseSettings:firebase_api_key").Value));
            this.userDbConfig = _userDbConfig;
        }


        public async Task RegisterUser(UserDtoRegister userDto)
        {
            FirebaseAuthLink firebaseAuthLink = await firebaseProvider.CreateUserWithEmailAndPasswordAsync(userDto.Email, userDto.Password);
            await SaveUserDb(userDto);
            // Here, send a message to the email service to confirm the user's email
            await firebaseProvider.SendEmailVerificationAsync(firebaseAuthLink);
        }

        private async Task SaveUserDb(UserDtoRegister userDto)
        {
            await userDbConfig.GetUserCollection().InsertOneAsync(
                new Models.User
                {
                    UserEmail = userDto.Email,
                    UserName = userDto.Name,
                });
            
        }
    }
}
