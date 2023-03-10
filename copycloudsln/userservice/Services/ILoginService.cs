using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using userservice.Dto;

namespace userservice.Services
{
    public interface ILoginService
    {
        Task<FirebaseAuthLink> Login(UserDtoLogin userDto);
    }
}
