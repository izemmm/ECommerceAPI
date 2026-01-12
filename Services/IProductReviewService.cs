using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IProductReviewService
    {
        
        Task<ServiceResponse<List<ProductReviewDto>>> GetReviewsByProductIdAsync(int productId);

    
        Task<ServiceResponse<List<ProductReviewDto>>> GetAllAsync();
    }
}