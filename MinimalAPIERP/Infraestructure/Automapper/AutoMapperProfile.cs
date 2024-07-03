using AutoMapper;
using ERP;
using Domain.Dtos;

namespace MinimalAPIERP.Infraestructure.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<CartItem, CartItemDto>().ReverseMap();

        }
    }
}

