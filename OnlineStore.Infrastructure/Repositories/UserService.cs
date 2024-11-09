using System.Net;
using System.Runtime.InteropServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class UserService(DataContext dataContext, IMapper mapper) : IUserService
{   
    private readonly DataContext _dataContext = dataContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<AllProductInfoDTO?>> GetProductById(int id){
        var product = await _dataContext.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        
        if(product is null) return new Response<AllProductInfoDTO?>(HttpStatusCode.Accepted, "Product is unavailable!!");

        return new Response<AllProductInfoDTO?>(_mapper.Map<AllProductInfoDTO>(product));
    }
    public async Task<Response<List<GetProductDTO?>>> GetAllProducts(){
        var products = await _dataContext.Products.ToListAsync();
        if(products is null) return new Response<List<GetProductDTO?>>(HttpStatusCode.BadRequest, "No products are available!");
        
        return new Response<List<GetProductDTO?>>(_mapper.Map<List<GetProductDTO?>>(products));
    }

    public async Task<Response<UserProfile?>> GetAccountInfo(int id)
    {
        var fullInfo = await _dataContext.UserProfiles
            .Where(x => x.Id == id)
            .Include(x => x.UserRoles)
        .FirstOrDefaultAsync();
        
        if(fullInfo is null)
            return new Response<UserProfile?>(HttpStatusCode.BadRequest, "Likely not authorized!");
        return new Response<UserProfile?>(fullInfo);
    }
}
