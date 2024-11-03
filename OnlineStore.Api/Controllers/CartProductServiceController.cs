using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Domain.Constants;

namespace OnlineStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize( Roles = Roles.User)]
public class CartProductServiceController(ICartProductService _cartProductService) : ControllerBase
{
    [HttpGet("get-all-cart-products{cartId}")]
    public async Task<ActionResult<List<CartProductDTO?>>> GetAllCartProducts(int cartId){
        int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier));
            
        var response = await _cartProductService.GetAllCartProducts(userProfileId, cartId);

        if(response.StatusCode == 200) return Ok(response.Data);
        else if(response.StatusCode == 400) return BadRequest(response.Errors);
        else return BadRequest(response.Errors);
    }

    [HttpDelete("delete-cart-product{cartId},{cartProductId}")]
    public async Task<ActionResult<string>> DeleteCartProduct(int cartId, int cartProductId){
        int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier));
            
        var response = await _cartProductService.DeleteCartProduct(userProfileId, cartId, cartProductId);

        if(response.StatusCode == 200)
            return Ok(response.Data);
        else
            if(response.StatusCode == 400) return BadRequest(response.Errors);
        else
            return BadRequest(response.Errors);
    }

    [HttpPut("update-cart-product{cartId}")]
    public async Task<ActionResult<string>> UpdateCartProduct(int cartId,[FromBody] CartProductDTO cartProductDTO){
        int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier));
            
        var response = await _cartProductService.UpdateCartProduct(userProfileId, cartId, cartProductDTO);

        if(response.StatusCode == 200)
            return Ok(response.Data);
        else
            if(response.StatusCode == 400) return BadRequest(response.Errors);
        else
            return BadRequest(response.Errors);
    }
}
