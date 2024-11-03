using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class UserService(DataContext dataContext, IMapper mapper) : IUserService
{   
    private readonly DataContext _dataContext = dataContext;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductDTO?> GetProductById(int id){
        var product = await _dataContext.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        if(product is null) return null;
        else{
            return new ProductDTO(){
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,                
                MarketplaceId = product.MarketplaceId
            };
        }
    }
    public async Task<List<ProductDTO>> GetAllProducts(){
        var products = await _dataContext.Products.ToListAsync();
        if(products is null) return null!;
        else return _mapper.Map<List<ProductDTO>>(products);
    }

    public async Task<UserProfile> GetAllMyInfo(int id){
        var fullInfo = await _dataContext.UserProfiles.Where(x => x.Id == id)
            .Include(x => x.User)
                .ThenInclude(x => x.Carts)
                    .ThenInclude(x => x.CartProducts)
            .Include(x => x.Seller)
                .ThenInclude(x => x!.Marketplaces)
                    .ThenInclude(x => x.Products)
            .Include(x => x.UserRoles)
            .FirstOrDefaultAsync();
        return fullInfo!;
    } 

}
