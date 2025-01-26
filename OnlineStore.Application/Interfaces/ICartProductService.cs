using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface ICartProductService
{
    public Task<Response<List<CartProductDTO?>>> GetAllCartProducts(int userAccountId, int cartId);  
    public Task<Response<string>> DeleteCartProduct(int userAccountId, int cartId, int cartProductId);
    public Task<Response<string>> UpdateCartProduct(int userAccountId, int cartId, CartProductDTO cartProductDTO);
}
