using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class ProductService(DataContext _dataContext, IMapper _mapper) : IProductService
{
    public async Task<Response<string>> CreateProduct(int userProfileId, int marketplaceId, ProductDTO productDTO) {
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
            ProductIcon = productDTO.ProductIcon,
            Attributes = productDTO.Attributes.Select( x =>
                new ProductAttribute(){Value = x.Value, CategoryAttributeId = x.CategoryAttributeId})
                    .ToList(),
            ProductImages = productDTO.ProductImages.Select( x =>
                new ProductImage(){ImageUrl = x.ImageUrl, IsPrimary = x.IsPrimary})
                    .ToList(),
        };

        await _dataContext.Products.AddAsync(product);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The product {productDTO.Name} was added to marketplace!");
    }

    public async Task<Response<string>> DeleteProduct(int userProfileId, int marketplaceId, int productId) {
        var seller = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
                .ThenInclude(m => m.Products)
        .FirstOrDefaultAsync();

        if (seller is null) return new Response<string>(HttpStatusCode.BadRequest, "Seller not found for the given user profile.");

        var product = seller.Marketplaces
            .Where(m => m.Id == marketplaceId)
            .SelectMany(m => m.Products)
        .FirstOrDefault(p => p.Id == productId);

        if (product is null) return new Response<string>(HttpStatusCode.BadRequest, "Product not found in seller's marketplaces.");

        _dataContext.Remove(product);
        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The {product.Name} product was deleted!");
    }

    public async Task<Response<List<ProductDTO?>>> GetAllProducts(int userProfileId, int marketplaceId) {
        var products = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
            .ThenInclude(m => m.Products)
            .SelectMany(x => x.Marketplaces.SelectMany(x => x.Products))
        .ToListAsync();

        if (products is null) return new Response<List<ProductDTO?>>(HttpStatusCode.BadRequest, "Products were not found in seller's marketplaces.");

        return new Response<List<ProductDTO?>>(_mapper.Map<List<ProductDTO?>>(products));
    }

    public async Task<Response<ProductDTO?>> GetProductById(int userProfileId, int marketplaceId, int productId) {
        var seller = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
            .ThenInclude(m => m.Products)
        .FirstOrDefaultAsync();

        if (seller is null) return new Response<ProductDTO?>(HttpStatusCode.BadRequest, "Seller not found for the given user profile.");

        var product = seller.Marketplaces
            .Where(x => x.Id == marketplaceId)
            .SelectMany(m => m.Products)
        .FirstOrDefault(p => p.Id == productId);

        if (product is null) return new Response<ProductDTO?>(HttpStatusCode.BadRequest, "Product not found in seller's marketplaces.");

        return new Response<ProductDTO?>(_mapper.Map<ProductDTO>(product));
    }

    // NOt best way to update the product but for now we say it's ok!
    public async Task<Response<string>> UpdateProduct(int userProfileId, int marketplaceId,  ProductDTO productDTO)
    {
        var seller = await _dataContext.Sellers
            .Where(s => s.UserProfileId == userProfileId)
            .Include(s => s.Marketplaces)
            .ThenInclude(x => x.Products)
        .FirstOrDefaultAsync();

        if (seller is null) return new Response<string>(HttpStatusCode.BadRequest, "the current Seller was not found!");

        var marketplace = seller.Marketplaces.FirstOrDefault(x => x.Id == marketplaceId);
        if(marketplace is null) return new Response<string>(HttpStatusCode.BadRequest, "The marketplace was not found!");

        var product = marketplace.Products.FirstOrDefault(x => x.Id == productDTO.Id);
        if(product is null) return new Response<string>(HttpStatusCode.BadRequest, "product wasn't found!");

        product.Name = productDTO.Name;
        product.Description = productDTO.Description;
        product.MarketplaceId = marketplaceId;
        product.Price = productDTO.Price;
        product.StockQuantity = productDTO.StockQuantity;
        product.SKU = "a";
        product.CategoryId = productDTO.CategoryId;
        product.ProductIcon = productDTO.ProductIcon;

        await _dataContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, $"The product {productDTO.Name} was added to marketplace!");    
    }
}