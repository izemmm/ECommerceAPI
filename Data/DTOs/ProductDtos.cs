namespace ECommerceAPI.DTOs
{
    // Kullanıcıya ürün listelerken göndereceğimiz model
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; } = string.Empty; 
    }

    // Yeni ürün eklerken isteyeceğimiz veriler
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }

    // Güncelleme yaparken isteyeceğimiz veriler
    public class UpdateProductDto
   {
        public string Name { get; set; } = "Örnek Ürün"; // Varsayılan isim
        public decimal Price { get; set; } = 100;        // Varsayılan fiyat
        public int Stock { get; set; } = 10;             // Varsayılan stok
        
        // İŞTE SİHİRLİ DOKUNUŞ BURADA:
        public int CategoryId { get; set; } = 1;         // Artık 0 değil, 1 gelecek!
    }
}