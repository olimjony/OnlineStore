using Microsoft.AspNetCore.Http;

namespace OnlineStore.Application.DTOs;

public class UpdateCartDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public IFormFile? CartIconURL { get; set; } = default!;
}
