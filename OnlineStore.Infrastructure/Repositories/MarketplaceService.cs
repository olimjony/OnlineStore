using System.Net;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Infrastructure.Persistence;
using OnlineStore.Domain.Entities;
using AutoMapper;
using Infrastructure.Services.FileService;
using OnlineStore.Domain.Constants;

namespace OnlineStore.Infrastructure.Repositories;

public class MarketplaceService(DataContext _dataContext, IMapper _mapper, IFileService _fileService) : IMarketplaceService
{
    public async Task<Response<List<GetMarketplaceDTO?>>> GetAllMarketplaces(int userAccountId){
        var marketplaces = await _dataContext.Sellers.Where(x => x.UserAccountId == userAccountId)
            .Include(x => x.Marketplaces)
            .SelectMany(x => x.Marketplaces)
        .ToListAsync();

        if (marketplaces is null)
            return new Response<List<GetMarketplaceDTO?>>(HttpStatusCode.BadRequest, "Unable to process request!");

        var marketplacesResponse = _mapper.Map<List<GetMarketplaceDTO?>>(marketplaces);
        return new Response<List<GetMarketplaceDTO?>>(marketplacesResponse);      
    }

    public async Task<Response<string>> CreateMarketplace(int userAccountId, CreateMarketplaceDTO marketplaceDTO){
        var seller = await _dataContext.Sellers
            .Where(x => x.UserAccountId == userAccountId)
        .FirstOrDefaultAsync();

        if(seller is null)
            return new Response<string>(HttpStatusCode.BadRequest, "Associated seller was not found!");
        
        var marketplace = new Marketplace(){
            Name = marketplaceDTO.Name,
            Description = marketplaceDTO.Description,
            SellerId = seller.Id
        };

        if(marketplaceDTO.IconURL != null){
            var iconFilePath = await _fileService.SaveFileAsync(Paths.marketplaceIconFolder, marketplaceDTO.IconURL);
            marketplace.IconURL = iconFilePath;
        }
        if(marketplaceDTO.ImageURL != null){
            var imageFilePath = await _fileService.SaveFileAsync(Paths.marketplaceImageFolder, marketplaceDTO.ImageURL);
            marketplace.ImageURL = imageFilePath;
        }
        
        
        _dataContext.Marketplaces.Add(marketplace);
        await _dataContext.SaveChangesAsync();
        
        return new Response<string>
            (HttpStatusCode.OK, $"You have successfully created a marketplace named {marketplace.Name}");

    }
    public async Task<Response<string>> DeleteMarketplace(int userAccountId, int marketplaceId){
        var marketplace = await _dataContext.Sellers
            .Where(s => s.UserAccountId == userAccountId)
            .SelectMany(s => s.Marketplaces)
        .FirstOrDefaultAsync(m => m.Id == marketplaceId);

        if(marketplace is null)
            return new Response<string>(HttpStatusCode.BadRequest, "The current marketplace doesnt exist!!");

        _dataContext.Marketplaces.Remove(marketplace);
        await _dataContext.SaveChangesAsync();

        return new Response<string>
            (HttpStatusCode.OK, $"The {marketplace.Name} was deleted successfully!");
    }

    public async Task<Response<AllMarketplaceInfoDTO?>> GetMarketplaceById(int userAccountId, int marketplaceId)
    {
        var marketplace = await _dataContext.Sellers
            .Where(s => s.UserAccountId == userAccountId)
            .SelectMany(s => s.Marketplaces)
        .FirstOrDefaultAsync(m => m.Id == marketplaceId);

        if(marketplace is null)
            return new Response<AllMarketplaceInfoDTO?>
                (HttpStatusCode.BadRequest, "The current marketplace doesnt exist!!");

        return new Response<AllMarketplaceInfoDTO?> (_mapper.Map<AllMarketplaceInfoDTO>(marketplace));
    }

    public async Task<Response<string>> UpdateMarketplace(int userAccountId, UpdateMarketplaceDTO marketplaceDTO)
    {
        var marketplace = await _dataContext.Sellers
            .Where(s => s.UserAccountId == userAccountId)
            .SelectMany(s => s.Marketplaces)
        .FirstOrDefaultAsync(m => m.Id == marketplaceDTO.Id);
        
        if(marketplace is null)
            return new Response<string>(HttpStatusCode.BadRequest, "The current marketplace doesnt exist!!");

        if(marketplaceDTO.Name != "" || marketplaceDTO.Name != null)
            marketplace.Name = marketplaceDTO.Name;

        if(marketplaceDTO.Description != "" || marketplaceDTO.Description != null)
            marketplace.Description = marketplaceDTO.Description; 
        
        if(marketplaceDTO.IconURL != null){
            var iconFilePath = await _fileService.SaveFileAsync(Paths.marketplaceIconFolder, marketplaceDTO.IconURL);
            marketplace.IconURL = iconFilePath;
        }
        if (marketplaceDTO.ImageURL != null) {
            var imageFilePath = await _fileService.SaveFileAsync(Paths.marketplaceImageFolder, marketplaceDTO.ImageURL);
            marketplace.ImageURL = imageFilePath;
        }
        
        await _dataContext.SaveChangesAsync();

        return new Response<string>
            (HttpStatusCode.OK, $"The {marketplace.Name} marketplace was updated!");
    }
}
