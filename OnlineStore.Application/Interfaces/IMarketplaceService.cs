using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IMarketplaceService
{
    public Task<Response<AllMarketplaceInfoDTO?>> GetMarketplaceById(int userAccountId, int marketplaceId);
    public Task<Response<List<GetMarketplaceDTO?>>> GetAllMarketplaces(int userAccountId);  
    public Task<Response<string>> CreateMarketplace(int userAccountId, CreateMarketplaceDTO marketplaceDTO);
    public Task<Response<string>> DeleteMarketplace(int userAccountId, int marketplaceId); 
    public Task<Response<string>> UpdateMarketplace(int userAccountId, UpdateMarketplaceDTO marketplaceDTO);
}
