namespace OnlineStore.Application.DTOs.AuthenticationDTOs;

public class ChangePasswordDTO
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string NewPasswordRepet { get; set; } = default!;
}
