namespace OnlineStore.Domain.Entities;

public class Seller 
{
    public int Id { get; set; }
    public int MaxMarketplaces { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public ICollection<Marketplace> Marketplaces { get; set; } = null!; 
}
