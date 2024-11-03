namespace OnlineStore.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = null!;
     public ICollection<CategoryAttribute> CategoryAttributes { get; set; } = null!;
}