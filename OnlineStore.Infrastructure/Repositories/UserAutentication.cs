using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Constants;
using OnlineStore.Application.Responses;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.DTOs;
using OnlineStore.Infrastructure.Persistence;
using System.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Services.HashService;
using OnlineStore.Application.DTOs.AuthenticationDTOs;
using OnlineStore.Infrastructure.Services.EmailService;
using Domain.DTOs.EmailDTOs;
using MimeKit.Text;
using Infrastructure.Services.FileService;

namespace OnlineStore.Infrastructure.Repositories;

public class UserAutentication(DataContext _dataContext, IConfiguration _configuration, IHashService _hashService, IEmailService _emailService, IFileService _fileService) : IUserAuthentication
{
    public async Task<Response<string>> Register(UserRegisterDTO userRegisterDTO)
    {
        var user = await _dataContext.UserAccounts
            .Where(x => x.Email == userRegisterDTO.Email)
        .FirstOrDefaultAsync();

        if (user is not null)
            return new Response<string>(HttpStatusCode.BadRequest, "User Already Exists!");

        int userRoleId = _dataContext.Roles
            .Where(x => x.Name == Roles.User)
        .First().Id;

        var profile = new UserAccount()
        {
            FirstName = userRegisterDTO.FirstName,
            LastName = userRegisterDTO.LastName,
            Email = userRegisterDTO.Email,
            PasswordHash = _hashService.ConvertToHash(userRegisterDTO.Password),
            User = new User() { MaxCarts = 3 },
            UserRoles = new List<UserRole> { new UserRole() { RoleId = userRoleId } }
        };

        await _dataContext.UserAccounts.AddAsync(profile);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "You were registered successfully");
    }

    public async Task<Response<string>> Login(UserLoginDTO userLoginDTO)
    {
        var user = await _dataContext.UserAccounts.Where(x => x.Email == userLoginDTO.Email).FirstOrDefaultAsync();

        if (user is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Email or Password is Wrong!");

        if (!_hashService.VerifyHash(userLoginDTO.Password, user.PasswordHash))
            return new Response<string>(HttpStatusCode.BadRequest, "Email or Password is Wrong!");

        return new Response<string>(await GenerateJwtToken(user));
    }

    public async Task<Response<string>> EnableSeller(int userAccountId)
    {
        var userAccount = await _dataContext.UserAccounts
            .Where(x => x.Id == userAccountId)
            .Include(x => x.UserRoles)
        .FirstOrDefaultAsync();

        if (userAccount is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error! Likely idunno what!");

        var sellerRole = await _dataContext.Roles.Where(x => x.Name == Roles.Seller).FirstOrDefaultAsync();

        foreach (var i in userAccount.UserRoles)
            if (i.RoleId == sellerRole?.Id)
                return new Response<string>(HttpStatusCode.BadRequest, "You are already a seller!");

        userAccount.Seller = new Seller()
        {
            MaxMarketplaces = 3,
            UserAccountId = userAccountId
        };

        userAccount.UserRoles.Add(
            new UserRole
            {
                RoleId = sellerRole!.Id,
                UserAccountId = userAccountId
            });

        await _dataContext.SaveChangesAsync();

        return new Response<string>(await GenerateJwtToken(userAccount));
    }

    private async Task<string> GenerateJwtToken(UserAccount userAccount)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
            new(ClaimTypes.Email, userAccount.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var roles = await _dataContext.UserRoles
            .Where(x => x.UserAccountId == userAccount.Id)
            .Include(x => x.Role)
        .ToListAsync();

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(2),
            signingCredentials: credentials
        );
        

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = securityTokenHandler.WriteToken(token);
        return tokenString!;
    }

    public async Task<Response<string>> ConfirmEmail(string emailAddress)
    {
        var userAccount = await _dataContext.UserAccounts
            .Where(x => x.Email == emailAddress)
        .FirstOrDefaultAsync();

        if (userAccount is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Current email doesnt exist!");

        var random = new Random();
        userAccount.ConfirmationCode = random.Next(100000, 999999).ToString();
        userAccount.ConfirmationDate = DateTime.UtcNow;

        await _dataContext.SaveChangesAsync();

        await _emailService.SendEmail(new EmailMessageDto([emailAddress], "reset password",
            $"<h1>{userAccount.ConfirmationCode}</h1> it expires at{userAccount.ConfirmationDate.Value.AddMinutes(3)}"), TextFormat.Html);

        return new Response<string>(HttpStatusCode.Accepted, "The confirmation code was sent!");
    }

    public async Task<Response<string>> ChangePassword (int userAccountId, ChangePasswordDTO passwordDTO)
    {
        var userAccount = await _dataContext.UserAccounts
            .Where(x => x.Id == userAccountId)
        .FirstOrDefaultAsync();

        if (userAccount is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unable to identify the user!");

        if (!_hashService.VerifyHash(passwordDTO.OldPassword, userAccount.PasswordHash))
            return new Response<string>(HttpStatusCode.BadRequest, "Wrong password bro!");

        if (passwordDTO.NewPassword != passwordDTO.NewPasswordRepet)
            return new Response<string>(HttpStatusCode.BadRequest, "New password and its repetead doesnt match!");

        userAccount.PasswordHash = _hashService.ConvertToHash(passwordDTO.NewPassword);

        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Your Password was sucessfully updated!");
    }

    public async Task<Response<string>> VerifyConfirmationCode(int userAccountId, string confirmationCode)
    {
        var userAccount = await _dataContext.UserAccounts
            .Where(x => x.Id == userAccountId)
        .FirstOrDefaultAsync();

        if(userAccount is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unable to identify the user!");
        
        if(DateTime.UtcNow > userAccount.ConfirmationDate.GetValueOrDefault().AddMinutes(3))
            return new Response<string>(HttpStatusCode.BadRequest, "Confirmation time is expired try again!");
        
        if(userAccount.ConfirmationCode != confirmationCode)
            return new Response<string>(HttpStatusCode.BadRequest, "Wrong confirmationCode! try later!");

        return new Response<string>(await GenerateJwtToken(userAccount));
    }

    public async Task<Response<string>> UpdateUser(int userAccountId, UserUpdateDTO userUpdateDTO)
    {
        var userAccount = await _dataContext.UserAccounts
            .Where(x => x.Id == userAccountId)
        .FirstOrDefaultAsync();

        if(userAccount is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unable to identify user!");

        if(userUpdateDTO.DateOfBirth is not null)
            userAccount.DateOfBirth = userUpdateDTO.DateOfBirth;
        
        if(userUpdateDTO.FirstName is not null)
            userAccount.FirstName = userUpdateDTO.FirstName;
        
        if(userUpdateDTO.LastName is not null)
            userAccount.FirstName = userUpdateDTO.LastName;

        if(userUpdateDTO.PhoneNumber is not null)
            userAccount.FirstName = userUpdateDTO.PhoneNumber;
        
        if(userUpdateDTO.FirstName is not null)
            userAccount.FirstName = userUpdateDTO.FirstName;
    
        if(userUpdateDTO.AccountImageURL != null) {
            var iconFilePath = await _fileService.SaveFileAsync(Paths.userAvatarFolder, userUpdateDTO.AccountImageURL);
            userAccount.AccountImageURL = iconFilePath;
        }

        return new Response<string>(HttpStatusCode.OK, "Profile was updated succesfully!");
    }
}
