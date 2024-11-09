using Microsoft.AspNetCore.Http;

namespace OnlineStore.Application.DTOs;

public class CreateProductDTO
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; } 

    public int CategoryId { get; set; }
    public int MarketplaceId { get; set; }

    public IFormFile ProductIconURL { get; set; } = null!;
    public List<IFormFile> ProductImages { get; set; } = new();
    public List<ProductAttributeDTO> Attributes { get; set; } = new();
}