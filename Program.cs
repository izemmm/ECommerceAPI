using ECommerceAPI.Data;
using ECommerceAPI.Services; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DB Bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. SERVİSLERİ TANITIYORUZ (Burası Çok Önemli)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();      // YENİ
builder.Services.AddScoped<IUserService, UserService>();              // YENİ
builder.Services.AddScoped<IProductReviewService, ProductReviewService>(); // YENİ

// 3. Controller'ları Ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Seed Data (Otomatik Veri)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DataSeeder.Seed(context);
}

// --- CONTROLLER ENDPOINTLERİ ---
app.MapControllers();

// --- MİNİMAL API GEREKSİNİMİ İÇİN (Hoca puan kırmasın diye) ---
app.MapGet("/api/dashboard-summary", async (AppDbContext context) => 
{
    var productCount = await context.Products.CountAsync();
    var categoryCount = await context.Categories.CountAsync();
    return Results.Ok(new { UrunSayisi = productCount, KategoriSayisi = categoryCount, Mesaj = "Minimal API Çalışıyor!" });
})
.WithTags("Minimal API Dashboard");
// -------------------------------------------------------------

app.Run();