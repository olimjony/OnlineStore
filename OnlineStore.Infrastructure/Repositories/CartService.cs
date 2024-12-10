using System.Net;
using AutoMapper;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class CartService(DataContext _dataContext, IMapper _mapper, IFileService _fileService) : ICartService
{
    public async Task<Response<string>> CreateCart(int userProfileId, CreateCartDTO cartDTO)
    {
        var user = await _dataContext.Users
            .Where(x => x.UserProfileId == userProfileId)
        .FirstOrDefaultAsync();

        if(user is null) return new Response<string>(HttpStatusCode.BadRequest, "Unable to identify the user!");

        var cart = new Cart(){
            Name = cartDTO.Name,
            Description = cartDTO.Description,
            UserId = userProfileId,
        };

        if (cartDTO.CartIconURL is not null) {
            var iconFilePath = await _fileService.SaveFileAsync(Paths.cartIconFolder, cartDTO.CartIconURL);
            cart.CartIconURL = iconFilePath;
        }

        _dataContext.Carts.Add(cart);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"New cart: {cartDTO.Name} was added!");
    }

    public async Task<Response<string>> DeleteCart(int userId, int cartId)
    {
        var user = await _dataContext.Users
            .Where(x => x.UserProfileId == userId)
            .Include(x => x.Carts)
        .FirstOrDefaultAsync();
        
        if(user is null) return new Response<string>(HttpStatusCode.BadRequest, "Unable to identify the user!");

        var cart = user.Carts.Where(x => x.Id == cartId).FirstOrDefault();
        if(cart is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Unable to find the cart!");

        _dataContext.Remove(cart);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The cart {cart.Name} was deleted!");
    }

    public async Task<Response<List<GetCartDTO?>>> GetAllCarts(int userProfileId)
    {
        var user = await _dataContext.Users
            .Where(x => x.UserProfileId == userProfileId)
            .Include(x => x.Carts)
        .FirstOrDefaultAsync();
        if(user is null)
            return new Response<List<GetCartDTO?>>(HttpStatusCode.BadRequest, "Unable to identify the user!");

        var cartsResponse = _mapper.Map<List<GetCartDTO>>(user.Carts);
        return new Response<List<GetCartDTO?>>(cartsResponse!);
    }

    public async Task<Response<AllCartInfoDTO?>> GetCartById(int userProfileId, int cartId)
    {
        var user = await _dataContext.Users
            .Where(x => x.UserProfileId == userProfileId)
            .Include(x => x.Carts)
        .FirstOrDefaultAsync();

        if(user is null)
            return new Response<AllCartInfoDTO?>(HttpStatusCode.BadRequest, "Unable to identify the user!");
        
        var cart = user.Carts.Where(x => x.Id == cartId).FirstOrDefault();
        
        if(cart is null) return new Response<AllCartInfoDTO?>(HttpStatusCode.BadRequest, "Cart wasnt found!");

        return new Response<AllCartInfoDTO?>(_mapper.Map<AllCartInfoDTO>(cart));
    }

    public async Task<Response<string>> UpdateCart(int userProfileId, UpdateCartDTO cartDTO)
    {
        var cart = await _dataContext.Users
            .Where(x => x.UserProfileId == userProfileId)
            .Include(x => x.Carts)
            .Select(x => x.Carts.FirstOrDefault(x => x.Id == cartDTO.Id))
        .FirstOrDefaultAsync();
        
        if(cart is null) return new Response<string>(HttpStatusCode.BadRequest, "Unknown Error!");

        if(!string.IsNullOrEmpty(cartDTO.Name))
            cart.Name = cartDTO.Name;
        
        if(!string.IsNullOrEmpty(cartDTO.Description))
            cart.Description = cartDTO.Description;
        
        cart.UserId = userProfileId;

        if (cartDTO.CartIconURL is not null) {
            var iconFilePath = await _fileService.SaveFileAsync(Paths.cartIconFolder, cartDTO.CartIconURL);
            cart.CartIconURL = iconFilePath;
        }

        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The cart {cart.Name} was deleted!");
    }
}
