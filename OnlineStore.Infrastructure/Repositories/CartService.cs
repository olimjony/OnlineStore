using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class CartService(DataContext _dataContext, IMapper _mapper) : ICartService
{
    public async Task<Response<string>> CreateCart(CartDTO cartDTO, int userId)
    {
        var user = await _dataContext.Users.Where(x => x.UserProfileId == userId).FirstOrDefaultAsync();
        if(user is null) return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error!");

        var cart = new Cart(){
            Name = cartDTO.Name,
            Description = cartDTO.Description,
            CartIconURL = cartDTO.CartIconURL,
            UserId = userId,
        };

        _dataContext.Carts.Add(cart);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "New Cart was Added!");
    }

    public async Task<Response<string>> DeleteCart(int userId, int cartId)
    {
        var cart = await _dataContext.Users
            .Where(x => x.UserProfileId == userId)
            .Include(x => x.Carts)
            .Select(x => x.Carts.FirstOrDefault(x => x.Id == cartId))
        .FirstOrDefaultAsync();
        
        if(cart is null) return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error!");

        _dataContext.Remove(cart);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The cart {cart.Name} was deleted!");
    }

    public async Task<Response<List<CartDTO?>>> GetAllCarts(int userProfileId)
    {
        var carts = await _dataContext.Users
            .Where(x => x.UserProfileId == userProfileId)
            .Include(x => x.Carts)
            .SelectMany(x => x.Carts)
        .ToListAsync();

        if(carts is null) return new Response<List<CartDTO?>>(HttpStatusCode.BadRequest, "Unknown Error!");

        var cartsResponse = _mapper.Map<List<CartDTO>>(carts);
        return new Response<List<CartDTO?>>(cartsResponse!);
    }

    public async Task<Response<CartDTO?>> GetCartById(int userId, int cartId)
    {
        var cart = await _dataContext.Users
            .Where(x => x.UserProfileId == userId)
            .Include(x => x.Carts)
            .Select(x => x.Carts.FirstOrDefault(x => x.Id == cartId))
        .FirstOrDefaultAsync();
        
        if(cart is null) return new Response<CartDTO?>(HttpStatusCode.BadRequest, "Unknown Error!");

        return new Response<CartDTO?>(_mapper.Map<CartDTO>(cart));
    }

    public async Task<Response<string>> UpdateCart(int userId, int cartId, CartDTO cartDTO)
    {
        var cart = await _dataContext.Users
            .Where(x => x.UserProfileId == userId)
            .Include(x => x.Carts)
            .Select(x => x.Carts.FirstOrDefault(x => x.Id == cartId))
        .FirstOrDefaultAsync();
        
        if(cart is null) return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error!");

        cart.Name = cartDTO.Name;
        cart.CartIconURL = cartDTO.CartIconURL;
        cart.Description = cartDTO.Description;
        cart.UserId = userId;

        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The cart {cart.Name} was deleted!");
    }
}
