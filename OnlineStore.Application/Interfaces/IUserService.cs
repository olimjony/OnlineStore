using OnlineStore.Application.DTOs;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Interfaces;

public interface IUserService{
    public Task<ProductDTO?> GetProductById(int id);
    public Task<List<ProductDTO>> GetAllProducts();
    public Task<UserProfile> GetAllMyInfo(int id);
}