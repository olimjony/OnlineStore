using System.Net;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Infrastructure.Persistence;
using OnlineStore.Domain.Entities;
using AutoMapper;

namespace OnlineStore.Infrastructure.Repositories;

public class MarketplaceService(DataContext _dataContext, IMapper _mapper) : IMarketplaceService
{
    public async Task<Response<List<MarketplaceDTO?>>> GetAllMarketplaces(int userProfileId){
        var marketplaces = await _dataContext.Sellers.Where(x => x.UserProfileId == userProfileId)
            .Include(x => x.Marketplaces)
            .SelectMany(x => x.Marketplaces)
        .ToListAsync();

        if (marketplaces is null)
            return new Response<List<MarketplaceDTO?>>(HttpStatusCode.BadRequest, "Unable to process request!");

        var marketplacesResponse = _mapper.Map<List<MarketplaceDTO?>>(marketplaces);
        return new Response<List<MarketplaceDTO?>>(marketplacesResponse);      
    }

    public async Task<Response<string>> CreateMarketplace(MarketplaceDTO marketplaceDTO, int userProfileId){
        var userProfile = await _dataContext.UserProfiles
            .Where(x => x.Id == userProfileId)
            .Include(x => x.Seller)
            .FirstOrDefaultAsync();

        if(userProfile is null) return new Response<string>(HttpStatusCode.BadRequest, "Unable to create marketplace!"); 
        
        var seller = userProfile.Seller;
        if(seller is null) return new Response<string>(HttpStatusCode.BadRequest, "Associated seller was not found!");
        
        var marketplace = new Marketplace(){
            Description = marketplaceDTO.Description,
            Name = marketplaceDTO.Name,
            ImageURL = marketplaceDTO.ImageURL,
            SellerId = userProfile.Seller!.Id
        };
        
        await _dataContext.Marketplaces.AddAsync(marketplace);
        await _dataContext.SaveChangesAsync();
        
        return new Response<string>(HttpStatusCode.OK, $"You have successfully created a marketplace named {marketplace.Name}");

    }
    public async Task<Response<string>> DeleteMarketplace(int userProfileId, int marketplaceId){
        var marketplace = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .SelectMany(s => s.Marketplaces)
        .FirstOrDefaultAsync(m => m.Id == marketplaceId);

        if(marketplace is null){
            return new Response<string>(HttpStatusCode.BadRequest, "The current marketplace doesnt exist!!");
        }

        _dataContext.Marketplaces.Remove(marketplace);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The {marketplace.Name} was deleted successfully!");
    }

    public async Task<Response<MarketplaceDTO?>> GetMarketplaceById(int userProfileId, int marketplaceId)
    {
        var marketplace = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .SelectMany(s => s.Marketplaces)
        .FirstOrDefaultAsync(m => m.Id == marketplaceId);

        if(marketplace is null){
            return new Response<MarketplaceDTO?>(HttpStatusCode.BadRequest, "The current marketplace doesnt exist!!");
        }

        return new Response<MarketplaceDTO?>(_mapper.Map<MarketplaceDTO>(marketplace));
    }

    public async Task<Response<string>> UpdateMarketplace(int userProfileId, MarketplaceDTO marketplaceDTO)
    {
        var marketplace = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .SelectMany(s => s.Marketplaces)
        .FirstOrDefaultAsync(m => m.Id == marketplaceDTO.Id);
        
        if(marketplace is null){
            return new Response<string>(HttpStatusCode.BadRequest, "The current marketplace doesnt exist!!");
        }

        marketplace.Name = marketplaceDTO.Name;
        marketplace.Description = marketplaceDTO.Description;
        marketplace.ImageURL = marketplaceDTO.ImageURL;
        
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The {marketplace.Name} marketplace was updated!");
    }
}
