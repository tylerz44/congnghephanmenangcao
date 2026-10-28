using FashionStore.Models;

namespace FashionStore.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                var ao = new Category { CategoryName = "Áo" };
                var quan = new Category { CategoryName = "Quần" };

                context.Categories.AddRange(ao, quan);
                context.SaveChanges();

                context.Products.AddRange(
                    new Product
                    {
                        ProductName = "Áo Hoodie Bape",
                        Price = 450000,
                        Quantity = 20,
                        Brand = "Bape",
                        ImageUrl = "/images/hoodie.jpg",
                        CategoryId = ao.CategoryId
                    },
                    new Product
                    {
                        ProductName = "Áo Thun Bape Star",
                        Price = 299000,
                        Quantity = 15,
                        Brand = "Bape",
                        ImageUrl = "/images/tshirt.jpg",
                        CategoryId = ao.CategoryId
                    }
                );

                context.SaveChanges();
            }
        }
    }
}