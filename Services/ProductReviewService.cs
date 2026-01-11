using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly AppDbContext _context;

        public ProductReviewService(AppDbContext context)
        {
            _context = context;
        }

        // 1. Bir ürünün yorumlarını getir
        public async Task<ServiceResponse<List<ProductReviewDto>>> GetReviewsByProductIdAsync(int productId)
        {
            var response = new ServiceResponse<List<ProductReviewDto>>();
            var reviews = await _context.ProductReviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId && !r.IsDeleted)
                .ToListAsync();

            response.Data = reviews.Select(r => new ProductReviewDto
            {
                Id = r.Id,
                Comment = r.Comment,
                Rating = r.StarCount, // ✅ DÜZELTİLDİ: Entity'deki isim StarCount
                ReviewerName = r.User != null ? r.User.FullName : "Anonim"
            }).ToList();

            return response;
        }

        // 2. TÜM yorumları getir (GetAllAsync)
        public async Task<ServiceResponse<List<ProductReviewDto>>> GetAllAsync()
        {
            var response = new ServiceResponse<List<ProductReviewDto>>();
            var reviews = await _context.ProductReviews
                .Include(r => r.User)
                .Where(r => !r.IsDeleted)
                .ToListAsync();

            response.Data = reviews.Select(r => new ProductReviewDto
            {
                Id = r.Id,
                Comment = r.Comment,
                Rating = r.StarCount, // ✅ DÜZELTİLDİ: Entity'deki isim StarCount
                ReviewerName = r.User != null ? r.User.FullName : "Anonim"
            }).ToList();

            response.Message = "Tüm yorumlar listelendi.";
            return response;
        }
    }
}