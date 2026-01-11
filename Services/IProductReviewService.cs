using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IProductReviewService
    {
        // Bir ürünün yorumlarını getir
        Task<ServiceResponse<List<ProductReviewDto>>> GetReviewsByProductIdAsync(int productId);

        // TÜM yorumları getir (Bu eksikti, ekliyoruz)
        Task<ServiceResponse<List<ProductReviewDto>>> GetAllAsync();
    }
}