using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface ICartService
{
    public Task<Response<AllCartInfoDTO?>> GetCartById(int userAccountId, int cartId);
    public Task<Response<List<GetCartDTO?>>> GetAllCarts(int userAccountId);  
    public Task<Response<string>> CreateCart(int userAccountId, CreateCartDTO cartDTO);
    public Task<Response<string>> DeleteCart(int userAccountId, int cartId); 
    public Task<Response<string>> UpdateCart(int userAccountId, UpdateCartDTO cartDTO);
}
