namespace OnlineStore.Application.DTOs;

public class AllMarketplaceInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set;} = default!;
    public string Description { get; set;} = default!;
    public bool? Approved { get; set;}
    public string IconURL { get; set; } = default!;
    public string ImageURL { get; set; } = default!;

    public int SellerId { get; set; }
}
