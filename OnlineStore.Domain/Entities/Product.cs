namespace OnlineStore.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; } 
    public string SKU { get; set; } = default!;

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public int MarketplaceId { get; set; }
    public Marketplace Marketplace { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string? ProductIcon { get; set; } = null!; 
    public ICollection<ProductImage> ProductImages { get; set; } = null!;    
    public ICollection<ProductAttribute> Attributes { get; set; } = null!;

    public ICollection<CartProduct> CartProducts { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = null!;
}
