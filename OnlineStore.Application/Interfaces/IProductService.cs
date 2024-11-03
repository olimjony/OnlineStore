using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IProductService
{   
    public Task<Response<ProductDTO?>> GetProductById(int userProfileId, int marketplaceId, int productId);
    public Task<Response<List<ProductDTO?>>> GetAllProducts(int userProfileId, int marketplaceId);  
    public Task<Response<string>> CreateProduct(int userProfileId, int marketplaceId, ProductDTO productDTO);
    public Task<Response<string>> DeleteProduct(int userProfileId, int mrketplaceId, int productId); 
    public Task<Response<string>> UpdateProduct(int userProfileId, int marketplaceId, ProductDTO productDTO);
}
