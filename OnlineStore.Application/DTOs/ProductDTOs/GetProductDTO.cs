namespace OnlineStore.Application.DTOs;

public class GetProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int StockQuantity { get; set; } 
    public decimal Price { get; set; }

    public string MarketplaceName { get; set; } = default!;

    public string ProductIconURL { get; set; } = null!; 
}
