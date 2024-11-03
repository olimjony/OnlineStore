namespace OnlineStore.Application.DTOs;

public class CartDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string CartIconURL { get; set; } = default!;
    
    public int UserId { get; set; }
}
