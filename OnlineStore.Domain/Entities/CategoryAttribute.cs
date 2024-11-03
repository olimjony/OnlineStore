namespace OnlineStore.Domain.Entities;

public class CategoryAttribute
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; }  = default!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
