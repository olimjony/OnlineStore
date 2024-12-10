using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Domain.Constants;
using OnlineStore.Application.Validators;
using OnlineStore.Application.Interfaces;

namespace OnlineStore.Api.Controllers;

[Authorize(Roles = Roles.Seller)]
[Route("api/[controller]")]
[ApiController]
public class ProductServiceController(IProductService _productService) : ControllerBase
{
    [HttpGet("get-product-by-id{marketplaceId}/{productId}")]
        public async Task<ActionResult<AllProductInfoDTO?>> GetProductById(int marketplaceId, int productId)
        {
            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var response = await _productService.GetProductById(userProfileId, marketplaceId, productId);

            if(response.StatusCode == 200)
                return Ok(response.Data);
            else if(response.StatusCode == 400) return BadRequest(response.Errors);
            
            return BadRequest(response.Errors);
        }

    [HttpGet("get-all-products{marketplaceId}")]
        public async Task<ActionResult<List<GetProductDTO?>>> GetAllProducts(int marketplaceId)
        {
            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var response = await _productService.GetAllProducts(userProfileId, marketplaceId);
            
            if(response.StatusCode == 200)
                return Ok(response.Data);
            else if(response.StatusCode == 400)
                return BadRequest(response.Errors);
            
            return BadRequest(response.Errors);
        }

    [HttpPost("create-product{marketplaceId}")]
        public async Task<ActionResult<string>> CreateProduct([FromForm] CreateProductDTO productDTO, int marketplaceId)
        {
            var validator = new CreateProductValidator();
            var validationResult = validator.Validate(productDTO);
            if( !validationResult.IsValid) return BadRequest(validationResult.ToString()); 

            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _productService.CreateProduct(userProfileId, marketplaceId, productDTO);

            if(response.StatusCode == 200)
                return Ok(response.Errors);
            else if(response.StatusCode == 400)
                return BadRequest(response.Errors);

            else return BadRequest(response.Errors);
        }

    [HttpDelete("delete-product-by-id{marketplaceId}/{productId}")]
        public async Task<ActionResult<string>> DeleteProduct(int marketplaceId, int productId)
        {
            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);   

            var response = await _productService.DeleteProduct(userProfileId, marketplaceId, productId);
            
            if(response.StatusCode == 200)
                return Ok(response.Errors);
            return BadRequest(response.Errors);
        }

    [HttpPut("update-product")]
        public async Task<ActionResult<string>> UpdateProduct([FromBody] UpdateProductDTO productDTO, int marketplaceId, int productId)
        {
            var validator = new UpdateProductValidator();
            var validationResult = validator.Validate(productDTO);
            if( !validationResult.IsValid) return BadRequest(validationResult.ToString());

            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _productService.UpdateProduct(userProfileId, productDTO);
            if(response.StatusCode == 200)
                return Ok(response.Errors);
            return BadRequest(response.Errors);
        }

}
