using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IProductService
{   
    public Task<Response<AllProductInfoDTO?>> GetProductById(int userAccountId, int marketplaceId, int productId);
    public Task<Response<List<GetProductDTO?>>> GetAllProducts(int userAccountId, int marketplaceId);  
    public Task<Response<string>> CreateProduct(int userAccountId, int marketplaceId, CreateProductDTO productDTO);
    public Task<Response<string>> DeleteProduct(int userAccountId, int marketplaceId, int productId); 
    public Task<Response<string>> UpdateProduct(int userAccountId, UpdateProductDTO productDTO);
}
