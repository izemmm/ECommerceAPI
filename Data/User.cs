using System.Text.Json.Serialization; 

namespace ECommerceAPI.Data
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

    
        [JsonIgnore] 
        public List<ProductReview> Reviews { get; set; } = new();
    }
}