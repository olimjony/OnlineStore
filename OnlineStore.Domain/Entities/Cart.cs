using System.Runtime.InteropServices;

namespace OnlineStore.Domain.Entities;
public class Cart
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? CartIconURL { get; set; } = default!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<CartProduct> CartProducts { get; set; } = null!;
}
