using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface ICartProductService
{
    public Task<Response<List<CartProductDTO?>>> GetAllCartProducts(int userProfileId, int cartId);  
    public Task<Response<string>> DeleteCartProduct(int userProfileId, int cartId, int cartProductId);
    public Task<Response<string>> UpdateCartProduct(int userProfileId, int cartId, CartProductDTO cartProductDTO);
}
