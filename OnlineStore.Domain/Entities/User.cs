namespace OnlineStore.Domain.Entities;
public class User
{
    public int Id { get; set; }
    public int MaxCarts { get; set; } = default!;

    public int UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public ICollection<Cart> Carts { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = null!;
}
