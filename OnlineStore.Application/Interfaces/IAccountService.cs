using OnlineStore.Application.DTOs.AccountDTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IAccountService
{
    public Task<Response<AllAccountInfoDTO?>> GetAccountInfo(int userAccountId);
    public Task<Response<string>> UpdateAccount(int userAccountId, UpdateAccountDTO account);

}
