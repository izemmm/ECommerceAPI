using ECommerceAPI.Data;

namespace ECommerceAPI.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Elektronik" },
                    new Category { Name = "Giyim" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

        
            if (!context.Products.Any())
            {
                var elektronik = context.Categories.First(c => c.Name == "Elektronik");
                var giyim = context.Categories.First(c => c.Name == "Giyim");

                var products = new List<Product>
                {
                    new Product { Name = "Laptop", Price = 25000, Stock = 10, CategoryId = elektronik.Id },
                    new Product { Name = "Tişört", Price = 500, Stock = 100, CategoryId = giyim.Id }
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}