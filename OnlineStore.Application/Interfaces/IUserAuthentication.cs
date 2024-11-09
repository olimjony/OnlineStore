using OnlineStore.Application.DTOs;
using OnlineStore.Application.DTOs.AuthenticationDTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IUserAuthentication
{
    public Task<Response<string>> Register(UserRegisterDTO userRegisterDTO);
    public Task<Response<string>> Login(UserLoginDTO userLoginDTO);
    public Task<Response<string>> EnableSeller(int userProfileId);
    public Task<Response<string>> ConfirmEmail(string emailAddress);
    public Task<Response<string>> VerifyConfirmationCode(int userProfileId, string confirmationCode);
    public Task<Response<string>> ChangePassword(int userProfileId, ChangePasswordDTO passwordDTO);
}
