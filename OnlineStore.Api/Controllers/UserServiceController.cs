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
    [Authorize(Roles = Roles.User + "," + Roles.Seller)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProductById(int id) {
        var product = await _productsService.GetProductById(id);
        if(product is null) return BadRequest("The current product doesnt exist anymore!");
        else return Ok(product);
    }

    [Authorize(Roles = Roles.User + "," + Roles.Seller)]
    [HttpPost("all")]
    public async Task<ActionResult<List<ProductDTO>>> GetAllProducts(){
        var products = await _productsService.GetAllProducts();
        if(products is null) return Ok("No products broo!");
        else return Ok(products);
    }

    [Authorize(Roles = Roles.User + "," + Roles.Seller)]
    [HttpPost("all-my-info")]
    public async Task<ActionResult<List<ProductDTO>>> GetAllInfos(){
        int userProfileId = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sub));
        var products = await _productsService.GetAllMyInfo(userProfileId);
        if(products is null) return Ok("No products broo!");
        else return Ok(products);
    }
    
}
