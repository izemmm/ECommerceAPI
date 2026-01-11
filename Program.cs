using ECommerceAPI.Data;
using ECommerceAPI.Services; 
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.DTOs;
using ECommerceAPI; 
using Microsoft.AspNetCore.Authentication.JwtBearer; // YENİ
using Microsoft.IdentityModel.Tokens; // YENİ
using System.Text; // YENİ

var builder = WebApplication.CreateBuilder(args);

// 1. DB Bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Servisler
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();

// ==========================================
// 🔐 3. JWT AUTHENTICATION AYARLARI (YENİ)
// ==========================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
// ==========================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// SIRALAMA ÖNEMLİ: Önce Kimlik Doğrulama (Authentication), Sonra Yetki (Authorization)
app.UseAuthentication(); // 🔐 YENİ EKLENDİ
app.UseAuthorization();

// Seed Data & Migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); 
    DataSeeder.Seed(context);
}

// ... (Minimal API Kodların Aynen Kalacak) ... 
// (Yer kaplamasın diye burayı kısalttım, senin kodunda Minimal API'leri silme!)

// Buraya Minimal API kodlarını yapıştırmayı unutma (Categories vs.)
// Eğer sildiysen önceki mesajımdan alabilirsin.
// Sadece yukarıdaki Authentication kısımlarını eklesen yeterli.

// MINIMAL API KISIMLARI (Aynı kalıyor - Yer kaplamasın diye kısalttım ama senin kodunda duruyor)
app.MapGet("/api/minimal/categories", async (AppDbContext context) => 
{
    var categories = await context.Categories.Where(c => !c.IsDeleted).ToListAsync();
    var dtos = categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();
    return Results.Ok(new ServiceResponse<List<CategoryDto>> { Data = dtos, Message = "Kategoriler listelendi." });
}).WithTags("Minimal API (Categories)");

app.MapPost("/api/minimal/categories", async (AppDbContext context, CreateCategoryDto request) => 
{
    var category = new Category { Name = request.Name };
    context.Categories.Add(category);
    await context.SaveChangesAsync();
    return Results.Created($"/api/minimal/categories/{category.Id}", new ServiceResponse<CategoryDto> { Data = new CategoryDto { Id = category.Id, Name = category.Name }, Message = "Eklendi" });
}).WithTags("Minimal API (Categories)");

app.MapPut("/api/minimal/categories/{id}", async (AppDbContext context, int id, CategoryDto request) => 
{
    var category = await context.Categories.FindAsync(id);
    if (category is null || category.IsDeleted) return Results.NotFound(new ServiceResponse<bool> { Success = false, Message = "Bulunamadı" });
    category.Name = request.Name;
    await context.SaveChangesAsync();
    return Results.Ok(new ServiceResponse<bool> { Data = true, Message = "Güncellendi" });
}).WithTags("Minimal API (Categories)");

app.MapDelete("/api/minimal/categories/{id}", async (AppDbContext context, int id) => 
{
    var category = await context.Categories.FindAsync(id);
    if (category is null || category.IsDeleted) return Results.NotFound(new ServiceResponse<bool> { Success = false, Message = "Bulunamadı" });
    category.IsDeleted = true; 
    await context.SaveChangesAsync();
    return Results.Ok(new ServiceResponse<bool> { Data = true, Message = "Silindi" });
}).WithTags("Minimal API (Categories)");

app.MapGet("/api/test/auth", (HttpContext context) => 
{
    if (!context.Request.Headers.ContainsKey("Sifre")) return Results.Unauthorized(); 
    return Results.Ok(new { message = "Giriş Başarılı" });
}).WithTags("Status Code Tests");

app.MapDelete("/api/test/nocontent", () => Results.NoContent()).WithTags("Status Code Tests");

app.MapControllers(); 
app.Run();