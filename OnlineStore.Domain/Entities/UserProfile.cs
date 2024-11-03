namespace OnlineStore.Domain.Entities;

public class UserProfile
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string? PhoneNumber { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public string? ProfileImageUrl { get; set; } = null!;

    public User User { get; set; } = null!; 
    public Seller? Seller { get; set; } 

    public ICollection<UserRole> UserRoles { get; set; } = null!;
}