namespace OnlineStore.Application.DTOs;

public class AllProductInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int StockQuantity { get; set; } 
    public decimal Price { get; set; }
    public string SKU { get; set; } = default!;

    public int MarketplaceId { get; set; }
    public int CategoryId { get; set; }

    public string ProductIconURL { get; set; } = null!; 
    public List<ProductImageDTO?> ProductImages { get; set; } = new List<ProductImageDTO?>();    
    public List<ProductAttributeDTO> Attributes { get; set; } = new List<ProductAttributeDTO>();
}
