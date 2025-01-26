using Microsoft.AspNetCore.Http;

namespace OnlineStore.Application.DTOs.AccountDTOs;

public class UpdateAccountDTO
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public IFormFile? AccountImageURL { get; set; } = null!;
}
