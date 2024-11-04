using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Interfaces;

public interface IUserService{
    public Task<Response<AllProductInfoDTO?>> GetProductById(int id);
    public Task<Response<List<AllProductDTO?>>> GetAllProducts();
    public Task<Response<UserProfile?>> GetAccountInfo(int id);
}