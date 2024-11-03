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
using AutoMapper;

namespace OnlineStore.Infrastructure.Repositories;

public class UserAutentication : IUserAuthentication
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public UserAutentication(DataContext dataContext, IConfiguration configuration, IMapper mapper)
    {
        _dataContext = dataContext;
        _configuration = configuration;
        _mapper = mapper;
    }
    public async Task<Response<string>> Register(UserRegisterDTO userRegisterDTO){
        var user = await _dataContext.UserProfiles
            .Where(x => x.Email == userRegisterDTO.Email)
        .FirstOrDefaultAsync();
        
        if(user is not null)
            return new Response<string>(HttpStatusCode.BadRequest, "User Already Exists!");
        
        int userRoleId = _dataContext.Roles
            .Where(x => x.Name == Roles.User)
        .First().Id;

        var profile = new UserProfile() {
            FirstName = userRegisterDTO.FirstName,
            LastName = userRegisterDTO.LastName,
            Email = userRegisterDTO.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password),
            User = new User(){ MaxCarts = 3 },
            UserRoles = new List<UserRole>{ new UserRole(){ RoleId = userRoleId } }
        };

        await _dataContext.UserProfiles.AddAsync(profile);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "You were registered successfully");
    }

    public async Task<Response<string>> Login(UserLoginDTO userLoginDTO){
        var user = await _dataContext.UserProfiles.Where(x => x.Email == userLoginDTO.Email).FirstOrDefaultAsync();

        if(user is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Email or Password is Wrong!");

        if( !BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.PasswordHash ))
            return new Response<string>(HttpStatusCode.BadRequest, "Email or Password is Wrong!");
        
        return new Response<string>(await GenerateJwtToken(user));
    }

    public async Task<Response<string>> EnableSeller(int userProfileId){
        var userProfile = await _dataContext.UserProfiles
            .Where(x => x.Id == userProfileId)
            .Include(x => x.UserRoles)
        .FirstOrDefaultAsync();
        
        if(userProfile is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error! Likely idunno what!");

        var sellerRole = await _dataContext.Roles.Where(x => x.Name == Roles.Seller).FirstOrDefaultAsync();        

        foreach(var i in userProfile.UserRoles)
            if(i.RoleId == sellerRole?.Id)
                return new Response<string>(HttpStatusCode.BadRequest, "You are already a seller!");
        
        userProfile.Seller = new Seller(){
            MaxMarketplaces = 3,
            UserProfileId = userProfileId
        };
        
        userProfile.UserRoles.Add(
            new UserRole{
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
}
