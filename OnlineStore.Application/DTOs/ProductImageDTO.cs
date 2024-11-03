namespace OnlineStore.Application.DTOs;

public class ProductImageDTO
{
    public string ImageUrl { get; set; } = default!;
    public bool IsPrimary { get; set; }
}