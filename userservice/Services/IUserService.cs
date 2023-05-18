namespace userservice.Services
{
    public interface IUserService
    {
        Task<Tuple<bool, string>> DeleteUser(string userEmail, string token);
    }
}
