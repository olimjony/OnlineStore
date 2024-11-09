namespace OnlineStore.Application.DTOs;

public class GetMarketplaceDTO
{
    public int Id { get; set; }
    public string Name { get; set;} = default!;
    public bool? Approved { get; set;}
    public string IconURL { get; set; } = default!;
}
