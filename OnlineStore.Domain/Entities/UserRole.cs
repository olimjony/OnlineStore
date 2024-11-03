namespace OnlineStore.Domain.Entities;

public class UserRole
{
    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}