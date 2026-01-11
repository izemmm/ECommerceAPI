using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("users")] // 🎯 İSTENEN URL YAPISI: /users
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /users
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserDto>>>> GetAll()
        {
            // UserService içinde bu metodun olduğunu varsayıyoruz (Standardına göre olmalı)
            var response = await _userService.GetAllUsersAsync();
            return Ok(response);
        }

        // GET: /users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<UserDto>>> GetSingle(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}