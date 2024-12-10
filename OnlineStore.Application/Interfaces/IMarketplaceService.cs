using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IMarketplaceService
{
    public Task<Response<AllMarketplaceInfoDTO?>> GetMarketplaceById(int userProfileId, int marketplaceId);
    public Task<Response<List<GetMarketplaceDTO?>>> GetAllMarketplaces(int userProfileId);  
    public Task<Response<string>> CreateMarketplace(int userProfileId, CreateMarketplaceDTO marketplaceDTO);
    public Task<Response<string>> DeleteMarketplace(int userProfileId, int marketplaceId); 
    public Task<Response<string>> UpdateMarketplace(int userProfileId, UpdateMarketplaceDTO marketplaceDTO);
}
