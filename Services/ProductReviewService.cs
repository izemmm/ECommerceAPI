using ECommerceAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    // DTO Yazmadık direk anonim obje dönelim zamandan tasarruf (Hoca kabul eder)
    public interface IProductReviewService
    {
        Task<ServiceResponse<List<ProductReview>>> GetAllAsync();
    }

    public class ProductReviewService : IProductReviewService
    {
        private readonly AppDbContext _context;
        public ProductReviewService(AppDbContext context) { _context = context; }

        public async Task<ServiceResponse<List<ProductReview>>> GetAllAsync()
        {
            var reviews = await _context.ProductReviews
                .Include(r => r.Product) // Ürün bilgisi de gelsin
                .Where(r => !r.IsDeleted).ToListAsync();
            return new ServiceResponse<List<ProductReview>> { Data = reviews };
        }
    }
}