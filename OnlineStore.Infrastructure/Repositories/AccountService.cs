using System;
using System.Net;
using AutoMapper;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs.AccountDTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Constants;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class AccountService(DataContext _dataContext, IMapper _mapper, IFileService _fileService) : IAccountService
{
    public async Task<Response<AllAccountInfoDTO?>> GetAccountInfo(int userAccountId)
    {
        var userAccount = await _dataContext.UserAccounts
            .Where(x => x.Id == userAccountId)
        .FirstOrDefaultAsync();

        if (userAccount == null)
            return new Response<AllAccountInfoDTO?>(HttpStatusCode.BadRequest,"Unable to authenticate the user!");
        return new Response<AllAccountInfoDTO?>(_mapper.Map<AllAccountInfoDTO>(userAccount));
    }

    public async Task<Response<string>> UpdateAccount(int userAccountId, UpdateAccountDTO accountDTO)
    {
        var userAccount = await _dataContext.UserAccounts
            .Where(x => x.Id == userAccountId)
        .FirstOrDefaultAsync();

        if (userAccount == null)
            return new Response<string>(HttpStatusCode.BadRequest,"Unable to authenticate the user!");
        
        if(!string.IsNullOrEmpty(accountDTO.FirstName)){
            userAccount.FirstName = accountDTO.FirstName;
        }

        if(!string.IsNullOrEmpty(accountDTO.LastName)){
            userAccount.LastName = accountDTO.LastName;
        }

        if(accountDTO.DateOfBirth is not null){
            userAccount.DateOfBirth = accountDTO.DateOfBirth;
        }

        if(accountDTO.AccountImageURL is not null) {
            var iconFilePath = await _fileService.SaveFileAsync(Paths.userAvatarFolder, accountDTO.AccountImageURL);
            userAccount.AccountImageURL = iconFilePath;
        }

        return new Response<string>
            (HttpStatusCode.OK, $"Your Account was updated!");  
    }
}
