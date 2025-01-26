using OnlineStore.Application.DTOs;
using OnlineStore.Application.DTOs.AuthenticationDTOs;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Interfaces;

public interface IUserAuthentication
{
    public Task<Response<string>> Register(UserRegisterDTO userRegisterDTO);
    public Task<Response<string>> UpdateUser(int userAccountId, UserUpdateDTO userUpdateDTO); 
    public Task<Response<string>> Login(UserLoginDTO userLoginDTO);
    public Task<Response<string>> EnableSeller(int userAccountId);
    public Task<Response<string>> ConfirmEmail(string emailAddress);
    public Task<Response<string>> VerifyConfirmationCode(int userAccountId, string confirmationCode);
    public Task<Response<string>> ChangePassword(int userAccountId, ChangePasswordDTO passwordDTO);
}
