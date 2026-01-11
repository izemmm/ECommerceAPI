using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductReviewService _reviewService;

        public ProductsController(IProductService productService, IProductReviewService reviewService)
        {
            _productService = productService;
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetAll()
        {
            var response = await _productService.GetAllProductsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<ProductDto>>> GetSingle(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<ServiceResponse<List<ProductReviewDto>>>> GetProductReviews(int id)
        {
            var response = await _reviewService.GetReviewsByProductIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ProductDto>>> CreateProduct(CreateProductDto request)
        {
            var response = await _productService.CreateProductAsync(request);
            if (!response.Success)
            {
                 // 🛠️ UYARI BURADA ÇÖZÜLÜYOR:
                 // Mesajın boş olmadığından emin oluyoruz, sonra içeriğine bakıyoruz.
                 if (!string.IsNullOrEmpty(response.Message) && response.Message.Contains("zaten mevcut")) 
                 {
                     return Conflict(response);
                 }
                 return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetSingle), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<ProductDto>>> UpdateProduct(int id, UpdateProductDto request)
        {
            var response = await _productService.UpdateProductAsync(id, request);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }
    }
}