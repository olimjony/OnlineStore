namespace OnlineStore.Domain.Entities;

public class UserRole
{
    public int UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}