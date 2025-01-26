namespace OnlineStore.Domain.Entities;

public class Seller 
{
    public int Id { get; set; }
    public int MaxMarketplaces { get; set; }

    public int UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public ICollection<Marketplace> Marketplaces { get; set; } = null!;
}
