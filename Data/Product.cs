using System.Text.Json.Serialization; // <-- BU ŞART

namespace ECommerceAPI.Data
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // İlişki 1: Kategori (Bir ürünün bir kategorisi olur)
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // İlişki 2: Yorumlar (Bir ürünün çok yorumu olur)
        [JsonIgnore] // <-- SONSUZ DÖNGÜYÜ ENGELLER
        public List<ProductReview> Reviews { get; set; } = new();
    }
}