using Microsoft.AspNetCore.Http;

namespace OnlineStore.Domain.Entities;

public class UserUpdateDTO
{
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public IFormFile AccountImageURL { get; set; } = null!;
}
