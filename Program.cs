using ECommerceAPI.Data;
using ECommerceAPI.Services; 
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.DTOs;
using ECommerceAPI; // Middleware'i görmek için

var builder = WebApplication.CreateBuilder(args);

// 1. DB ve Servisler (Veritabanı Bağlantısı)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servislerin Tanımlanması (Dependency Injection)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// 🔥 GLOBAL EXCEPTION HANDLER (Hata Yakalayıcı)
// ==========================================
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// ==========================================
// 🛠️ SEED DATA VE MIGRATION (Değişen Kısım)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // EnsureCreated() YERİNE Migrate() KULLANIYORUZ
    // Bu komut, "Migrations" klasöründeki talimatlara göre veritabanını günceller.
    context.Database.Migrate(); 
    
    DataSeeder.Seed(context);
}

// ==================================================================
// 🔥 MINIMAL API - CRUD (DTO + ServiceResponse STANDARDI)
// ==================================================================

// 1. LİSTELE (Read)
app.MapGet("/api/minimal/categories", async (AppDbContext context) => 
{
    var categories = await context.Categories.Where(c => !c.IsDeleted).ToListAsync();
    var dtos = categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();
    return Results.Ok(new ServiceResponse<List<CategoryDto>> { Data = dtos, Message = "Kategoriler listelendi." });
})
.WithTags("Minimal API (Categories)");

// 2. EKLE (Create)
app.MapPost("/api/minimal/categories", async (AppDbContext context, CreateCategoryDto request) => 
{
    var category = new Category { Name = request.Name };
    context.Categories.Add(category);
    await context.SaveChangesAsync();
    return Results.Created($"/api/minimal/categories/{category.Id}", new ServiceResponse<CategoryDto> { Data = new CategoryDto { Id = category.Id, Name = category.Name }, Message = "Eklendi" });
})
.WithTags("Minimal API (Categories)");

// 3. GÜNCELLE (Update)
app.MapPut("/api/minimal/categories/{id}", async (AppDbContext context, int id, CategoryDto request) => 
{
    var category = await context.Categories.FindAsync(id);
    if (category is null || category.IsDeleted) return Results.NotFound(new ServiceResponse<bool> { Success = false, Message = "Bulunamadı" });
    
    category.Name = request.Name;
    await context.SaveChangesAsync();
    return Results.Ok(new ServiceResponse<bool> { Data = true, Message = "Güncellendi" });
})
.WithTags("Minimal API (Categories)");

// 4. SİL (Delete)
app.MapDelete("/api/minimal/categories/{id}", async (AppDbContext context, int id) => 
{
    var category = await context.Categories.FindAsync(id);
    if (category is null || category.IsDeleted) return Results.NotFound(new ServiceResponse<bool> { Success = false, Message = "Bulunamadı" });
    
    category.IsDeleted = true; // Soft Delete
    await context.SaveChangesAsync();
    return Results.Ok(new ServiceResponse<bool> { Data = true, Message = "Silindi" });
})
.WithTags("Minimal API (Categories)");

// ==================================================================
// 🚦 STATUS CODE DEMO ENDPOINTLERİ (204 ve 401 Örnekleri)
// ==================================================================

// 1. 401 Unauthorized Örneği
app.MapGet("/api/test/auth", (HttpContext context) => 
{
    if (!context.Request.Headers.ContainsKey("Sifre"))
    {
        return Results.Unauthorized(); 
    }
    return Results.Ok(new { message = "Giriş Başarılı" });
})
.WithTags("Status Code Tests");

// 2. 204 No Content Örneği
app.MapDelete("/api/test/nocontent", () => 
{
    return Results.NoContent();
})
.WithTags("Status Code Tests");
// ==================================================================

app.MapControllers(); 
app.Run();