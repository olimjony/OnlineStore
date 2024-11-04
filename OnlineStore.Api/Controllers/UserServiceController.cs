using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Domain.Constants;

namespace OnlineStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsServiceController(IUserService _productsService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProductById(int id) {
        var response = await _productsService.GetProductById(id);
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        return BadRequest(response.Errors);
    }

    [HttpPost("all")]
    public async Task<ActionResult<List<ProductDTO>>> GetAllProducts(){
        var response = await _productsService.GetAllProducts();
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        return BadRequest(response.Errors);
    }

    [Authorize(Roles = Roles.User + "," + Roles.Seller)]
    [HttpPost("all-my-info")]
    public async Task<ActionResult<List<ProductDTO>>> GetAllInfos(){
        int userProfileId = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sub));
        var response = await _productsService.GetAccountInfo(userProfileId);
        
        if(response.StatusCode == 200)
            return Ok(response.Data);
        return BadRequest(response.Errors);
    }
    
}
