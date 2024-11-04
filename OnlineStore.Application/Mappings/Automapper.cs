using AutoMapper;
using OnlineStore.Domain.Entities;
using OnlineStore.Application.DTOs;

namespace OnlineStore.Application.Mapping
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, AllProductInfoDTO>().ReverseMap();
            CreateMap<Product, AllProductDTO>().ReverseMap();
            CreateMap<Marketplace, MarketplaceDTO>();
            CreateMap<Cart, CartDTO>();
            CreateMap<CartProduct, CartProductDTO>()
                .ForMember(dest => dest.ProductDTO, opt => opt.MapFrom(src => src.Product));
        }
    }
}