namespace ECommerceAPI.Services
{
    // Hocanın istediği standart cevap kalıbı
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}