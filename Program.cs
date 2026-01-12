using ECommerceAPI.Data;
using ECommerceAPI.Services; 
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.DTOs;
using ECommerceAPI; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();


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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ECommerce API", Version = "v1" });
});

var app = builder.Build();


app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1"));
}

app.UseHttpsRedirection();


app.UseAuthentication(); 
app.UseAuthorization();  


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); 
    DataSeeder.Seed(context);
}


app.MapGet("/api/minimal/categories", async (AppDbContext context) => 
{
    var categories = await context.Categories.Where(c => !c.IsDeleted).ToListAsync();
    var dtos = categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();
    return Results.Ok(new ServiceResponse<List<CategoryDto>> { Data = dtos, Message = "Listelendi" });
}).WithTags("Minimal API").RequireAuthorization();

app.MapControllers(); 
app.Run();