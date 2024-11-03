namespace OnlineStore.Domain.Entities;

public class ProductAttribute
{
    public int Id { get; set; }

    public string Value { get; set; } = default!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int CategoryAttributeId { get; set; }
    public CategoryAttribute CategoryAttribute { get; set; } = null!; 
}