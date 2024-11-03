using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IMarketplaceService
{
    public Task<Response<MarketplaceDTO?>> GetMarketplaceById(int userProfileId, int marketplaceId);
    public Task<Response<List<MarketplaceDTO?>>> GetAllMarketplaces(int userProfileId);  
    public Task<Response<string>> CreateMarketplace(MarketplaceDTO marketplaceDTO,int userId);
    public Task<Response<string>> DeleteMarketplace(int userProfileId, int marketplaceId); 
    public Task<Response<string>> UpdateMarketplace(int userProfileId, MarketplaceDTO marketplaceDTO);
}
