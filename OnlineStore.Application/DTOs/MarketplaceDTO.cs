namespace OnlineStore.Application.DTOs;

public class MarketplaceDTO
{
    public int Id { get; set; }
    public string Name { get; set;} = default!;
    public string Description { get; set;} = default!;
    public bool? Approved { get; set;}
    public string ImageURL { get; set; } = default!;

    public int SellerId { get; set; }
}
