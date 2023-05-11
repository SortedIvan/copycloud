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


        public async Task<Tuple<bool, string>> RegisterUser(UserDtoRegister userDto)
        {
            try
            {
                bool userExists = await userDbConfig.CheckUserExists(userDto.Email);
                if (userExists)
                {
                    return Tuple.Create(false, "A user with this email already exists");
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }

            FirebaseAuthLink firebaseAuthLink = await firebaseProvider.CreateUserWithEmailAndPasswordAsync(userDto.Email, userDto.Password);
            Tuple<bool, string> result = await userDbConfig.SaveUserDb(userDto, firebaseAuthLink.User.LocalId);

            // Here, send a message to the email service to confirm the user's email
            await firebaseProvider.SendEmailVerificationAsync(firebaseAuthLink);

            return result;
        }


    }
}
