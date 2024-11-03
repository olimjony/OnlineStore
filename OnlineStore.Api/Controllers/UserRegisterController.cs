using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Validators;
using OnlineStore.Domain.Constants;

namespace OnlineStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRegisterController(IUserAuthentication userAuthentication) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register-user")]
    public async Task<ActionResult<string>> RegisterAsUser([FromBody] UserRegisterDTO userRegisterDTO) {
        var validator = new UserRegisterValidator();
        var validationResult = validator.Validate(userRegisterDTO);
        if( !validationResult.IsValid) return BadRequest(validationResult.ToString()); 
        
        var response = await userAuthentication.Register(userRegisterDTO);
        if(response.StatusCode == 200) return Ok(response.Errors);
        else return BadRequest(response.Errors);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserLoginDTO userLoginDTO){
        var validator = new UserLoginValidator();
        var validationResult = validator.Validate(userLoginDTO);
        if(!validationResult.IsValid)
            return BadRequest(validationResult.ToString()); 
        
        var response = await userAuthentication.Login(userLoginDTO);
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        else
            return BadRequest(response.Errors);
    }

    [Authorize(Roles = Roles.User)]
    [HttpPut("enable-seller")]
    public async Task<ActionResult<string>> EnableSeller(){
        int Id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        var response = await userAuthentication.EnableSeller(Id);
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        else
            return BadRequest(response.Errors);
    }
}
