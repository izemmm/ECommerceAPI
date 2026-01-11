using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetAllProductsAsync();
        Task<ServiceResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto productDto);
        Task<ServiceResponse<bool>> DeleteProductAsync(int id);
    }
}