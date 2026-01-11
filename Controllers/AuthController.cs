using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _userService.LoginAsync(request);
            if (!response.Success)
            {
                return BadRequest(response); // 400 Hata
            }
            return Ok(response); // 200 + Token
        }
    }
}