using Microsoft.AspNetCore.Http;

namespace OnlineStore.Application.DTOs;

public class CreateCartDTO
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public IFormFile? CartIconURL { get; set; } = default!;
}