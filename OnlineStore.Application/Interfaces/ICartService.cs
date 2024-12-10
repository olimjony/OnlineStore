using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface ICartService
{
    public Task<Response<AllCartInfoDTO?>> GetCartById(int userProfileId, int cartId);
    public Task<Response<List<GetCartDTO?>>> GetAllCarts(int userProfileId);  
    public Task<Response<string>> CreateCart(int userProfileId, CreateCartDTO cartDTO);
    public Task<Response<string>> DeleteCart(int userProfileId, int cartId); 
    public Task<Response<string>> UpdateCart(int userProfileId, UpdateCartDTO cartDTO);
}
