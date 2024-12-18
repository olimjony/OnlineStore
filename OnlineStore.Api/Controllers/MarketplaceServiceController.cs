using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Validators;
using OnlineStore.Domain.Constants;

namespace OnlineStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Seller)]
    public class MarketplaceController(IMarketplaceService _marketplaceService) : ControllerBase
    {
        [HttpGet("get-marketplace-by-id{marketplaceId}")]
        public async Task<ActionResult<AllMarketplaceInfoDTO?>> GetMarketplaceById(int marketplaceId)
        {
            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var response = await _marketplaceService.GetMarketplaceById(userProfileId, marketplaceId);

            if(response.StatusCode == 200) return Ok(response.Data);
            else if(response.StatusCode == 400) return BadRequest(response.Errors);
            else return BadRequest(response.Errors);
        }

        [HttpGet("get-all-marketplaces")]
        public async Task<ActionResult<List<GetMarketplaceDTO?>>> GetAllMarketplaces()
        {
            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var response = await _marketplaceService.GetAllMarketplaces(userProfileId);
            
            if(response.StatusCode == 200) return Ok(response.Data);
            else return BadRequest(response.Errors);
        }

        [HttpPost("create-marketplace")]
        public async Task<ActionResult<string>> CreateMarketplace([FromBody] CreateMarketplaceDTO marketplaceDTO)
        {
            var validator = new CreateMarketplaceValidator();
            var validationResult = validator.Validate(marketplaceDTO);
            if( !validationResult.IsValid) return BadRequest(validationResult.ToString()); 

            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _marketplaceService.CreateMarketplace(userProfileId, marketplaceDTO);

            if(response.StatusCode == 200) return Ok(response.Errors);
            else return BadRequest(response.Errors);
        }

        [HttpDelete("delete-marketplace-by-id{marketplaceId}")]
        public async Task<ActionResult<string>> DeleteMarketplace(int marketplaceId)
        {
            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);   

            var response = await _marketplaceService.DeleteMarketplace(userProfileId, marketplaceId);
            
            if(response.StatusCode == 200) return Ok(response.Errors);
            else return BadRequest(response.Errors);
        }

        [HttpPut("update-marketplace{marketplaceId}")]
        public async Task<ActionResult<string>> UpdateMarketplace([FromBody] UpdateMarketplaceDTO marketplaceDTO, int marketplaceId)
        {
            var validator = new UpdateMarketplaceValidator();
            var validationResult = validator.Validate(marketplaceDTO);
            if( !validationResult.IsValid) return BadRequest(validationResult.ToString());

            int userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _marketplaceService.UpdateMarketplace(userProfileId, marketplaceDTO);
            if(response.StatusCode == 200) return Ok(response.Errors);
            else return BadRequest(response.Errors);
        }
    }
}