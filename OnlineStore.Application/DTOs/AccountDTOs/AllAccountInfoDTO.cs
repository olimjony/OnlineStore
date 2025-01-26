namespace OnlineStore.Application.DTOs.AccountDTOs;

public class AllAccountInfoDTO
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public string? AccountImageURL { get; set; } = null!;
}
