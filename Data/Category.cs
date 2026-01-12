using System.Text.Json.Serialization; 

namespace ECommerceAPI.Data
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        
        [JsonIgnore] 
        public List<Product> Products { get; set; } = new();
    }
}