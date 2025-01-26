using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Domain.Constants;
using OnlineStore.Application.Validators;
using OnlineStore.Application.Interfaces;

namespace OnlineStore.Api.Controllers;

[Authorize(Roles = Roles.User)]
[Route("api/[controller]")]
[ApiController]
public class CartServiceController(ICartService _cartService) : ControllerBase
{
    [HttpGet("get-cart-by-id{cartId}")]
        public async Task<ActionResult<AllCartInfoDTO?>> GetCartById(int cartId)
        {
            int userAccountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var response = await _cartService.GetCartById(userAccountId, cartId);

            if(response.StatusCode == 200) return Ok(response.Data);
            else if(response.StatusCode == 400) return BadRequest(response.Errors);
            else return BadRequest(response.Errors);
        }

    [HttpGet("get-all-carts")]
        public async Task<ActionResult<List<GetCartDTO?>>> GetAllCarts()
        {
            int userAccountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var response = await _cartService.GetAllCarts(userAccountId);
            
            if(response.StatusCode == 200) return Ok(response.Data);
            else return BadRequest(response.Errors);
        }

        [HttpPost("create-cart")]
        public async Task<ActionResult<string>> CreateCart([FromForm] CreateCartDTO cartDTO)
        {
            var validator = new CreateCartValidator();
            var validationResult = validator.Validate(cartDTO);
            if( !validationResult.IsValid) return BadRequest(validationResult.ToString()); 

            int userAccountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Console.WriteLine(userAccountId);

            var response = await _cartService.CreateCart(userAccountId, cartDTO);

            if(response.StatusCode == 200) return Ok(response.Errors);
            else return BadRequest(response.Errors);
        }

    [HttpDelete("delete-cart-by-id{cartId}")]
        public async Task<ActionResult<string>> DeleteCart(int cartId)
        {
            int userAccountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _cartService.DeleteCart(userAccountId, cartId);
            
            if(response.StatusCode == 200) return Ok(response.Errors);
            else return BadRequest(response.Errors);
        }

    [HttpPut("update-cart{cartId}")]
        public async Task<ActionResult<string>> Cart([FromBody] UpdateCartDTO cartDTO, int cartId)
        {
            var validator = new UpdateCartValidator();
            var validationResult = validator.Validate(cartDTO);
            if( !validationResult.IsValid) return BadRequest(validationResult.ToString());

            int userAccountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _cartService.UpdateCart(userAccountId, cartDTO);
            if(response.StatusCode == 200) return Ok(response.Errors);
            else return BadRequest(response.Errors);
        }

}
