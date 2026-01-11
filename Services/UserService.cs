using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserDto>>> GetAllAsync();
        Task<ServiceResponse<UserDto>> CreateAsync(CreateUserDto request);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context) { _context = context; }

        public async Task<ServiceResponse<List<UserDto>>> GetAllAsync()
        {
            var users = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            var dtos = users.Select(u => new UserDto { 
                Id = u.Id, FullName = u.FullName, Email = u.Email, Role = u.Role 
            }).ToList();
            return new ServiceResponse<List<UserDto>> { Data = dtos };
        }

        public async Task<ServiceResponse<UserDto>> CreateAsync(CreateUserDto request)
        {
            var user = new User { 
                FullName = request.FullName, Email = request.Email, Password = request.Password, Role = "User" 
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new ServiceResponse<UserDto> { 
                Data = new UserDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role } 
            };
        }
    }
}