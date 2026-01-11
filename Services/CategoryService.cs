using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<CategoryDto>>> GetAllAsync();
        Task<ServiceResponse<CategoryDto>> CreateAsync(CreateCategoryDto request);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            var dtos = categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();
            return new ServiceResponse<List<CategoryDto>> { Data = dtos };
        }

        public async Task<ServiceResponse<CategoryDto>> CreateAsync(CreateCategoryDto request)
        {
            var category = new Category { Name = request.Name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new ServiceResponse<CategoryDto> { Data = new CategoryDto { Id = category.Id, Name = category.Name } };
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return new ServiceResponse<bool> { Success = false, Message = "Kategori bulunamadı." };

            category.IsDeleted = true; // Soft Delete
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true, Message = "Kategori silindi." };
        }
    }
}