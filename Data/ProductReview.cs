namespace ECommerceAPI.Data
{
    public class ProductReview : BaseEntity
    {
        public string Comment { get; set; } = string.Empty;
        public int StarCount { get; set; } // Puan (1-5)

        // Bağlantı 1: Hangi Kullanıcı Yazdı?
        public int UserId { get; set; }
        public User? User { get; set; }

        // Bağlantı 2: Hangi Ürüne Yazıldı?
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}