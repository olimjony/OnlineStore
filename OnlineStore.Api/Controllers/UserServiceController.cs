using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsServiceController(IUserService _productsService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<ActionResult<AllProductInfoDTO>> GetProductById(int id) {
        var response = await _productsService.GetProductById(id);
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        return BadRequest(response.Errors);
    }

    [HttpPost("all")]
    public async Task<ActionResult<List<GetProductDTO>>> GetAllProducts(){
        var response = await _productsService.GetAllProducts();
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        return BadRequest(response.Errors);
    }

    [Authorize(Roles = Roles.User + "," + Roles.Seller)]
    [HttpPost("all-my-info")]
    public async Task<ActionResult<UserProfile?>> GetAllInfos(){
        int userProfileId = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sub));
        var response = await _productsService.GetAccountInfo(userProfileId);
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        return BadRequest(response.Errors);
    }
}
