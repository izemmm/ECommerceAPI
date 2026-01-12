using System.Text.Json.Serialization; 

namespace ECommerceAPI.Data
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        
        [JsonIgnore] 
        public List<ProductReview> Reviews { get; set; } = new();
    }
}