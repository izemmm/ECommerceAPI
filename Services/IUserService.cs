using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IUserService
    {
        // Controller'ın çağıracağı isimler bunlardır:
        Task<ServiceResponse<List<UserDto>>> GetAllUsersAsync();
        Task<ServiceResponse<UserDto>> GetUserByIdAsync(int id);
        
        Task<ServiceResponse<UserDto>> CreateUserAsync(CreateUserDto userDto);
        Task<ServiceResponse<string>> LoginAsync(UserLoginDto request);
    }
}