using userservice.Dto;

namespace userservice.Services
{
    public interface IRegisterService
    {
        Task<Tuple<bool, string>> RegisterUser(UserDtoRegister userDto);
    }
}
