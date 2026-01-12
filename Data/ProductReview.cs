namespace ECommerceAPI.Data
{
    public class ProductReview : BaseEntity
    {
        public string Comment { get; set; } = string.Empty;
        public int StarCount { get; set; } 

        
        public int UserId { get; set; }
        public User? User { get; set; }

        
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}