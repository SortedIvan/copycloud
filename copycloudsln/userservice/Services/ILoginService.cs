using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;

namespace userservice.Services
{
    public interface ILoginService
    {
        Task<FirebaseAuthLink> Login(string username, string password);
    }
}
