using AutoMapper;
using OnlineStore.Domain.Entities;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.DTOs.AccountDTOs;

namespace OnlineStore.Application.Mapping
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            //account
            CreateMap<UserAccount, AllAccountInfoDTO>().ReverseMap();

            //mapping for products
            CreateMap<Product, AllProductInfoDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, GetProductDTO>().ReverseMap();

            //mapping for marketplaces
            CreateMap<Marketplace, CreateMarketplaceDTO>();
            CreateMap<Marketplace, AllMarketplaceInfoDTO>();
            CreateMap<Marketplace, GetMarketplaceDTO>();

            //mapping for carts
            CreateMap<Cart, CreateCartDTO>();
            CreateMap<Cart, AllCartInfoDTO>();
            CreateMap<Cart, GetCartDTO>();

            CreateMap<CartProduct, CartProductDTO>()
                .ForMember(dest => dest.ProductDTO, opt => opt.MapFrom(src => src.Product));
        }
    }
}