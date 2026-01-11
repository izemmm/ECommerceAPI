using System.Net;
using System.Text.Json;
using ECommerceAPI.Services; // ServiceResponse için gerekli

namespace ECommerceAPI
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // İsteği normal akışına bırak
                await _next(context);
            }
            catch (Exception ex)
            {
                // Hata olursa yakala ve logla
                _logger.LogError(ex, "Beklenmedik bir hata oluştu!");
                
                // Özel cevap hazırla
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Hocanın istediği standart formatta hata mesajı
            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = $"Sunucu hatası: {exception.Message}", // Gerçek hatayı gösteriyoruz (Hoca görsün diye)
                Data = null
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}