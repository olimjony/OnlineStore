using AutoMapper;
using OnlineStore.Domain.Entities;
using OnlineStore.Application.DTOs;

namespace OnlineStore.Application.Mapping
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<Product, AllProductInfoDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();

            CreateMap<Marketplace, CreateMarketplaceDTO>();
            CreateMap<Marketplace, AllMarketplaceInfoDTO>();
            CreateMap<Marketplace, GetMarketplaceDTO>();

            CreateMap<Cart, CartDTO>();
            CreateMap<CartProduct, CartProductDTO>()
                .ForMember(dest => dest.ProductDTO, opt => opt.MapFrom(src => src.Product));
        }
    }
}