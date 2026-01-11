using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetAllProductsAsync()
        {
            var response = new ServiceResponse<List<ProductDto>>();
            
            // SADECE SILINMEMIS (!IsDeleted) OLANLARI GETIR
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted) 
                .ToListAsync();

            response.Data = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CategoryName = p.Category != null ? p.Category.Name : "Yok"
            }).ToList();

            return response;
        }

        public async Task<ServiceResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            var response = new ServiceResponse<ProductDto>();
            
            // SILINMEMIS OLANLAR ICINDE ARA
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Ürün bulunamadı.";
                return response;
            }

            response.Data = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category?.Name ?? "Yok"
            };
            return response;
        }

        public async Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto productDto)
        {
            var response = new ServiceResponse<ProductDto>();

            var newProduct = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            response.Data = new ProductDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Price = newProduct.Price,
                Stock = newProduct.Stock
            };
            response.Message = "Ürün başarıyla oluşturuldu.";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            var product = await _context.Products.FindAsync(id);

            if (product == null || product.IsDeleted)
            {
                response.Success = false;
                response.Message = "Silinecek ürün bulunamadı.";
                response.Data = false;
            }
            else
            {
                // BONUS: SOFT DELETE (IsDeleted = true yapıyoruz)
                product.IsDeleted = true;
                product.UpdatedAt = DateTime.Now;
                
                await _context.SaveChangesAsync();
                response.Data = true;
                response.Message = "Ürün başarıyla kaldırıldı (Soft Delete).";
            }
            return response;
        }
    }
}