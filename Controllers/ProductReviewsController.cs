using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs; // DTO namespace'ini eklemeyi unutma

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewsController : ControllerBase
    {
        private readonly IProductReviewService _service;
        
        public ProductReviewsController(IProductReviewService service) 
        { 
            _service = service; 
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductReviewDto>>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}