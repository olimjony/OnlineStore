namespace OnlineStore.Application.DTOs;

public class CartProductDTO
{
    public int Id;
    public int Quantity { get; set;}
    public bool Liked { get; set;}

    public DateTime DateAdded { get; set;}
    public DateTime DateUpdated { get; set; }

    public int CartId { get; set; }
    
    public int ProductId { get; set; }
    public ProductDTO ProductDTO { get; set; } = null!;
}
