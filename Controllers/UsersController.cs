using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service) { _service = service; }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserDto>>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<UserDto>>> Create(CreateUserDto request)
        {
            return Ok(await _service.CreateAsync(request));
        }
    }
}