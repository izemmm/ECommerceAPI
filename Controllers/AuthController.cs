using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;
using Microsoft.AspNetCore.Authorization; //

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // 🔓 Giriş yapmak için token gerekmez
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _userService.LoginAsync(request);
            if (!response.Success)
            {
                return BadRequest(response); 
            }
            return Ok(response); 
        }
    }
}