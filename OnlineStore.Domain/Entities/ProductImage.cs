namespace OnlineStore.Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = default!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}