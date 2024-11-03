namespace OnlineStore.Domain.Entities;
public class User
{
    public int Id { get; set; }
    public int MaxCarts { get; set; } = default!;

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public ICollection<Cart> Carts { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = null!;
}
