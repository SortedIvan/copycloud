using userservice.Dto;

namespace userservice.Services
{
    public interface IRegisterService
    {
        Task RegisterUser(UserDtoRegister userDto);
    }
}
