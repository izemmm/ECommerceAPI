using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetAllProductsAsync();
        Task<ServiceResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto productDto);
        Task<ServiceResponse<bool>> DeleteProductAsync(int id);
        
        // EKSİK OLAN GÜNCELLEME METODU:
        Task<ServiceResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto productDto);
    }
}