using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CategoryDto>>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<CategoryDto>>> Create(CreateCategoryDto request)
        {
            return Ok(await _service.CreateAsync(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(int id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
    }
}