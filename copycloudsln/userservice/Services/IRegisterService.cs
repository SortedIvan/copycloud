namespace userservice.Services
{
    public interface IRegisterService
    {
        Task RegisterUser(string username, string password);
    }
}
