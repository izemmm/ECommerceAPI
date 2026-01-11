using System.Text.Json.Serialization; // <-- BU SATIR ÇOK ÖNEMLİ

namespace ECommerceAPI.Data
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // İlişki: Bir kategorinin altında bir sürü ürün olabilir.
        // [JsonIgnore] ekliyoruz ki verileri çekerken sonsuz döngüye girmesin.
        [JsonIgnore] 
        public List<Product> Products { get; set; } = new();
    }
}