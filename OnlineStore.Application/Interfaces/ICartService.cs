using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface ICartService
{
    public Task<Response<CartDTO?>> GetCartById(int userProfileId, int cartId);
    public Task<Response<List<CartDTO?>>> GetAllCarts(int userProfileId);  
    public Task<Response<string>> CreateCart(CartDTO cartDTO,int userId);
    public Task<Response<string>> DeleteCart(int userProfileId, int cartId); 
    public Task<Response<string>> UpdateCart(int userProfileId, int cartId, CartDTO cartDTO);
}
