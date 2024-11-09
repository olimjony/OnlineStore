using System.Net;
using AutoMapper;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class ProductService(DataContext _dataContext, IMapper _mapper, IFileService _fileService) : IProductService
{
    public async Task<Response<string>> CreateProduct(int userProfileId, int marketplaceId, CreateProductDTO productDTO) {
        var seller = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
        .FirstOrDefaultAsync();

        if (seller is null) return new Response<string>(HttpStatusCode.BadRequest, "the current Seller was not found!");

        var marketplace = seller.Marketplaces.FirstOrDefault(x => x.Id == marketplaceId);
        if(marketplace is null) return new Response<string>(HttpStatusCode.BadRequest, "The marketplace was not found!");

        var product = new Product(){
            Name = productDTO.Name,
            Description = productDTO.Description,
            MarketplaceId = marketplaceId,
            Price = productDTO.Price,
            StockQuantity = productDTO.StockQuantity,
            SKU = "a",
            CategoryId = productDTO.CategoryId,
            ProductImages = new List<ProductImage>(),
            Attributes = productDTO.Attributes.Select( x =>
                new ProductAttribute(){Value = x.Value, CategoryAttributeId = x.CategoryAttributeId})
            .ToList(),
        };

        if (productDTO.ProductIconURL is not null) {
            var iconFilePath = await _fileService.SaveFileAsync(productDTO.ProductIconURL);
            product.ProductIcon = iconFilePath;
        }

        if(productDTO.ProductImages is not null){
            foreach (var image in productDTO.ProductImages) {
                var imagePath = await _fileService.SaveFileAsync(image);
                product.ProductImages.Add(new ProductImage { ImageUrl = imagePath });
            }
        }

        _dataContext.Products.Add(product);
        await _dataContext.SaveChangesAsync();

        return new Response<string>
            (HttpStatusCode.OK, $"The product {productDTO.Name} was added to marketplace!");
    }

    public async Task<Response<string>> DeleteProduct(int userProfileId, int marketplaceId, int productId) {
        var seller = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
                .ThenInclude(m => m.Products)
        .FirstOrDefaultAsync();

        if (seller is null) return new Response<string>
            (HttpStatusCode.BadRequest, "Seller not found for the given user profile.");

        var product = seller.Marketplaces
            .Where(m => m.Id == marketplaceId)
            .SelectMany(m => m.Products)
        .FirstOrDefault(p => p.Id == productId);

        if (product is null) return new Response<string>
            (HttpStatusCode.BadRequest, "Product not found in seller's marketplaces.");

        _dataContext.Remove(product);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The {product.Name} product was deleted!");
    }

    public async Task<Response<List<GetProductDTO?>>> GetAllProducts(int userProfileId, int marketplaceId) {
        var products = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .SelectMany(x => x.Marketplaces)
            .SelectMany(x => x.Products)
        .ToListAsync();

        if (products is null) return new Response<List<GetProductDTO?>>
            (HttpStatusCode.BadRequest, "Products were not found in seller's marketplaces.");

        return new Response<List<GetProductDTO?>>(_mapper.Map<List<GetProductDTO?>>(products));
    }

    public async Task<Response<AllProductInfoDTO?>> GetProductById(int userProfileId, int marketplaceId, int productId) {
        var seller = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
            .ThenInclude(m => m.Products)
        .FirstOrDefaultAsync();

        if (seller is null) return new Response<AllProductInfoDTO?>
            (HttpStatusCode.BadRequest, "Seller not found for the given user profile.");

        var product = seller.Marketplaces
            .Where(x => x.Id == marketplaceId)
            .SelectMany(m => m.Products)
        .FirstOrDefault(p => p.Id == productId);

        if (product is null) return new Response<AllProductInfoDTO?>
            (HttpStatusCode.BadRequest, "Product not found in seller's marketplaces.");

        return new Response<AllProductInfoDTO?>(_mapper.Map<AllProductInfoDTO>(product));
    }

    public async Task<Response<string>> UpdateProduct(int userProfileId, int marketplaceId, int productId, CreateProductDTO productDTO)
    {
        var seller = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
            .ThenInclude(x => x.Products)
        .FirstOrDefaultAsync();

        if (seller is null) return new Response<string>
            (HttpStatusCode.BadRequest, "the current Seller was not found!");

        var marketplace = seller.Marketplaces.FirstOrDefault(x => x.Id == marketplaceId);
        if(marketplace is null) return new Response<string>
            (HttpStatusCode.BadRequest, "The marketplace was not found!");

        var product = marketplace.Products.FirstOrDefault(x => x.Id == productId);
        if(product is null) return new Response<string>
            (HttpStatusCode.BadRequest, "product wasn't found!");

        if(!string.IsNullOrEmpty(productDTO.Name))
            product.Name = productDTO.Name;

        if(!string.IsNullOrEmpty(productDTO.Description))
            product.Description = productDTO.Description;

        if(productDTO.Price != 0 && !(productDTO.Price < 0))
            product.Price = productDTO.Price;

        if(productDTO.StockQuantity != 0 && !(productDTO.StockQuantity < 0))
            product.StockQuantity = productDTO.StockQuantity;

        if(productDTO.MarketplaceId != 0 && !(productDTO.MarketplaceId < 0))
            product.MarketplaceId = marketplaceId;
        
        if(productDTO.CategoryId != 0 && !(productDTO.CategoryId < 0))
            product.CategoryId = productDTO.CategoryId;
        
        product.SKU = "a";
        
        if(productDTO.ProductIconURL != null) {
            var iconFilePath = await _fileService.SaveFileAsync(productDTO.ProductIconURL);
            product.ProductIcon = iconFilePath;
        }

        if(productDTO.ProductImages != null){
            foreach (var image in productDTO.ProductImages) {
                var imagePath = await _fileService.SaveFileAsync(image);
                product.ProductImages.Add(new ProductImage { ImageUrl = imagePath });
            }
        }

        return new Response<string>
            (HttpStatusCode.OK, $"The product {productDTO.Name} was added to marketplace!");    
    }
}
