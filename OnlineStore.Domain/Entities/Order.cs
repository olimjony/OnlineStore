namespace OnlineStore.Domain.Entities;
public class Order
{   
    public int Id { get; set;}
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime OrderDate { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
