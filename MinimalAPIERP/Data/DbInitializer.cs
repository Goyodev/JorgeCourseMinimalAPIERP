using ERP;
using ERP.Data;

namespace MinimalAPIERP.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context, ILogger logger)
        {
            // Usar transacciones para garantizar la atomicidad
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Evitar duplicados verificando si ya hay datos
                if (!context.Categories.Any() && !context.Products.Any() && !context.CartItems.Any())
                {
                    var categories = new List<Category>();
                    var products = new List<Product>();
                    var cartItems = new List<CartItem>();

                    for (int i = 1; i <= 10; i++)
                    {
                        var category = new Category
                        {
                            Name = $"Category {i}",
                            Description = $"Description for Category {i}",
                            ImageUrl = $"http://example.com/images/category{i}.jpg"
                        };
                        categories.Add(category);

                        for (int j = 1; j <= 10; j++)
                        {
                            var product = new Product
                            {
                                SkuNumber = $"SKU-{i * 10 + j}",
                                Category = category,
                                Title = $"Product {i * 10 + j}",
                                Price = (decimal)(i * 10 + j) * 1.99M,
                                ProductArtUrl = $"http://example.com/images/product{i * 10 + j}.jpg",
                                Description = $"Description for Product {i * 10 + j}",
                                Created = DateTime.UtcNow,
                                ProductDetails = $"Details for Product {i * 10 + j}",
                                Inventory = 100
                            };
                            products.Add(product);
                        }

                        for (int k = 1; k <= 2; k++)
                        {
                            var cartItem = new CartItem
                            {
                                Product = products[i * 10 - 10 + k - 1],
                                Count = k,
                                DateCreated = DateTime.UtcNow
                            };
                            cartItems.Add(cartItem);
                        }
                    }

                    await context.Categories.AddRangeAsync(categories);
                    await context.Products.AddRangeAsync(products);
                    await context.CartItems.AddRangeAsync(cartItems);
                    await context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                logger.LogInformation("Database has been initialized");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }
    }
}
