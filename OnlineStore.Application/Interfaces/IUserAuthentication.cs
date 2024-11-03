using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IUserAuthentication
{
    public Task<Response<string>> Register(UserRegisterDTO userRegisterDTO);
    public Task<Response<string>> Login(UserLoginDTO userLoginDTO);
    public Task<Response<string>> EnableSeller(int Id);
    //Task<Response<string>> ForgotPasswordCodeGenerator(ForgotPasswordDTO forgotPasswordDTO);
    //Task<Response<string>> ChangePassword(ChangePasswordDTO passwordDTO, int userId);
}
