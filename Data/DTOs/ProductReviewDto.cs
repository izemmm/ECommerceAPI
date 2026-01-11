namespace ECommerceAPI.DTOs
{
    public class ProductReviewDto
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string ReviewerName { get; set; } = string.Empty; // Yorumu yapan kişinin adı
    }
}