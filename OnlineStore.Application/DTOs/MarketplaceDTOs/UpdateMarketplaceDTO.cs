using Microsoft.AspNetCore.Http;

namespace OnlineStore.Application.DTOs;

public class UpdateMarketplaceDTO
{
    public int Id { get; set; }
    public string Name { get; set;} = default!;
    public string Description { get; set;} = default!;
    public bool? Approved { get; set;}
    public IFormFile? IconURL { get; set; } = default!;
    public IFormFile? ImageURL { get; set; } = default!;
}
