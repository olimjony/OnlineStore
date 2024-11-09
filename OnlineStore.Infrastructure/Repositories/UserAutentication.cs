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

namespace OnlineStore.Infrastructure.Repositories;

public class UserAutentication(DataContext _dataContext, IConfiguration _configuration, IHashService _hashService, IEmailService _emailService) : IUserAuthentication
{
    public async Task<Response<string>> Register(UserRegisterDTO userRegisterDTO)
    {
        var user = await _dataContext.UserProfiles
            .Where(x => x.Email == userRegisterDTO.Email)
        .FirstOrDefaultAsync();

        if (user is not null)
            return new Response<string>(HttpStatusCode.BadRequest, "User Already Exists!");

        int userRoleId = _dataContext.Roles
            .Where(x => x.Name == Roles.User)
        .First().Id;

        var profile = new UserProfile()
        {
            FirstName = userRegisterDTO.FirstName,
            LastName = userRegisterDTO.LastName,
            Email = userRegisterDTO.Email,
            PasswordHash = _hashService.ConvertToHash(userRegisterDTO.Password),
            User = new User() { MaxCarts = 3 },
            UserRoles = new List<UserRole> { new UserRole() { RoleId = userRoleId } }
        };

        await _dataContext.UserProfiles.AddAsync(profile);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "You were registered successfully");
    }

    public async Task<Response<string>> Login(UserLoginDTO userLoginDTO)
    {
        var user = await _dataContext.UserProfiles.Where(x => x.Email == userLoginDTO.Email).FirstOrDefaultAsync();

        if (user is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Email or Password is Wrong!");

        if (!_hashService.VerifyHash(userLoginDTO.Password, user.PasswordHash))
            return new Response<string>(HttpStatusCode.BadRequest, "Email or Password is Wrong!");

        return new Response<string>(await GenerateJwtToken(user));
    }

    public async Task<Response<string>> EnableSeller(int userProfileId)
    {
        var userProfile = await _dataContext.UserProfiles
            .Where(x => x.Id == userProfileId)
            .Include(x => x.UserRoles)
        .FirstOrDefaultAsync();

        if (userProfile is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error! Likely idunno what!");

        var sellerRole = await _dataContext.Roles.Where(x => x.Name == Roles.Seller).FirstOrDefaultAsync();

        foreach (var i in userProfile.UserRoles)
            if (i.RoleId == sellerRole?.Id)
                return new Response<string>(HttpStatusCode.BadRequest, "You are already a seller!");

        userProfile.Seller = new Seller()
        {
            MaxMarketplaces = 3,
            UserProfileId = userProfileId
        };

        userProfile.UserRoles.Add(
            new UserRole
            {
                RoleId = sellerRole!.Id,
                UserProfileId = userProfileId
            });

        await _dataContext.SaveChangesAsync();

        return new Response<string>(await GenerateJwtToken(userProfile));
    }

    private async Task<string> GenerateJwtToken(UserProfile userProfile)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
            new(ClaimTypes.Email, userProfile.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var roles = await _dataContext.UserRoles
            .Where(x => x.UserProfileId == userProfile.Id)
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
        var userProfile = await _dataContext.UserProfiles
            .Where(x => x.Email == emailAddress)
        .FirstOrDefaultAsync();

        if (userProfile is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Current email doesnt exist!");

        var random = new Random();
        userProfile.ConfirmationCode = random.Next(100000, 999999).ToString();
        userProfile.ConfirmationDate = DateTime.UtcNow;

        await _dataContext.SaveChangesAsync();

        await _emailService.SendEmail(new EmailMessageDto([emailAddress], "reset password",
            $"<h1>{userProfile.ConfirmationCode}</h1> it expires at{userProfile.ConfirmationDate.Value.AddMinutes(3)}"), TextFormat.Html);

        return new Response<string>(HttpStatusCode.Accepted, "The confirmation code was sent!");
    }

    public async Task<Response<string>> ChangePassword (int userProfileId, ChangePasswordDTO passwordDTO)
    {
        var userProfile = await _dataContext.UserProfiles
            .Where(x => x.Id == userProfileId)
        .FirstOrDefaultAsync();

        if (userProfile is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unable to identify the user!");

        if (!_hashService.VerifyHash(passwordDTO.OldPassword, userProfile.PasswordHash))
            return new Response<string>(HttpStatusCode.BadRequest, "Wrong password bro!");

        if (passwordDTO.NewPassword != passwordDTO.NewPasswordRepet)
            return new Response<string>(HttpStatusCode.BadRequest, "New password and its repetead doesnt match!");

        userProfile.PasswordHash = _hashService.ConvertToHash(passwordDTO.NewPassword);

        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Your Password was sucessfully updated!");
    }

    public async Task<Response<string>> VerifyConfirmationCode(int userProfileId, string confirmationCode)
    {
        var userProfile = await _dataContext.UserProfiles
            .Where(x => x.Id == userProfileId)
        .FirstOrDefaultAsync();

        if(userProfile is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unable to identify the user!");
        
        if(DateTime.UtcNow > userProfile.ConfirmationDate.GetValueOrDefault().AddMinutes(3))
            return new Response<string>(HttpStatusCode.BadRequest, "Confirmation time is expired try again!");
        
        if(userProfile.ConfirmationCode != confirmationCode)
            return new Response<string>(HttpStatusCode.BadRequest, "Wrong confirmationCode! try later!");

        return new Response<string>(await GenerateJwtToken(userProfile));
    }
}
