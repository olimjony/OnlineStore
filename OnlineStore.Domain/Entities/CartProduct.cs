namespace OnlineStore.Domain.Entities;

public class CartProduct{
    public int Id;
    public int Quantity { get; set;}
    public bool Liked { get; set;}

    public DateTime DateAdded { get; set;}
    public DateTime DateUpdated { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}