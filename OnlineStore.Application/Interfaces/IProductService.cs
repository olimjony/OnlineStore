using OnlineStore.Application.DTOs;
using OnlineStore.Application.Responses;

namespace OnlineStore.Application.Interfaces;

public interface IProductService
{   
    public Task<Response<AllProductInfoDTO?>> GetProductById(int userProfileId, int marketplaceId, int productId);
    public Task<Response<List<GetProductDTO?>>> GetAllProducts(int userProfileId, int marketplaceId);  
    public Task<Response<string>> CreateProduct(int userProfileId, int marketplaceId, CreateProductDTO productDTO);
    public Task<Response<string>> DeleteProduct(int userProfileId, int marketplaceId, int productId); 
    public Task<Response<string>> UpdateProduct(int userProfileId, UpdateProductDTO productDTO);
}
