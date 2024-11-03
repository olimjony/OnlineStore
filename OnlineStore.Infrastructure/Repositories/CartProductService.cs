using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class CartProductService(DataContext _dataContext, IMapper _mapper) : ICartProductService
{
    public async Task<Response<string>> DeleteCartProduct(int userProfileId, int cartId, int cartProductId)
    {
        var cartProduct = await _dataContext.Users
            .Where(x => x.UserProfileId == userProfileId)
            .SelectMany(x => x.Carts)
            .SelectMany(c => c.CartProducts)
        .FirstOrDefaultAsync(cp => cp.Id == cartProductId);

        if(cartProduct is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error!");

        _dataContext.CartProducts.Remove(cartProduct);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The Cart Product {cartProduct.Id} was deleted sucessfully!");
    }

    public async Task<Response<List<CartProductDTO?>>> GetAllCartProducts(int userProfileId, int cartId)
    {
        var cartProducts = await _dataContext.Users
            .Where(x => x.UserProfileId == userProfileId)
            .SelectMany(x => x.Carts.Where(x => x.Id == cartId))
            .SelectMany(x => x.CartProducts)
        .ToListAsync();
        
        if(cartProducts is null)
            return new Response<List<CartProductDTO?>>(HttpStatusCode.BadRequest, "Unknown Error!");
        
        var cartProductResponse = _mapper.Map<List<CartProductDTO?>>(cartProducts);

        return new Response<List<CartProductDTO?>>(cartProductResponse);
    }

    public async Task<Response<string>> UpdateCartProduct(int userProfileId, int cartId, CartProductDTO cartProductDTO)
    {
        var cartProduct = await _dataContext.Users
            .Where(x => x.Id == userProfileId)
            .SelectMany(x => x.Carts.Where(x => x.Id == cartId))
            .SelectMany(x => x.CartProducts.Where(x => x.Id == cartProductDTO.Id))
        .FirstOrDefaultAsync();

        if(cartProduct is null)
            return new Response<string>(HttpStatusCode.BadRequest, "The current product deosnt exist in your cart!");
    
        cartProduct.Liked = cartProductDTO.Liked;
        cartProduct.CartId = cartProductDTO.CartId;
        cartProduct.Quantity = cartProductDTO.Quantity;

        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "The product cart was updated!");
    }
}
