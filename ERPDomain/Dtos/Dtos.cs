namespace Domain.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string SkuNumber { get; set; } = null!;
        public int CategoryId { get; set; }
        public int RecommendationId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public string? ProductArtUrl { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Created { get; set; }
        public string ProductDetails { get; set; } = null!;
        public int Inventory { get; set; }
        public Guid ProductGuid { get; set; } = Guid.NewGuid();
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public Guid CategoryGuid { get; set; } = Guid.NewGuid();
    }
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public string CartId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
