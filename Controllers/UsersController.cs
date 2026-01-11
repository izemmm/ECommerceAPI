using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("users")] // 🎯 İSTENEN URL: /users
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet] // GET /users
        public async Task<ActionResult<ServiceResponse<List<UserDto>>>> GetAll()
        {
            var response = await _userService.GetAllUsersAsync();
            return Ok(response);
        }

        [HttpGet("{id}")] // GET /users/5
        public async Task<ActionResult<ServiceResponse<UserDto>>> GetSingle(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }
    }
}