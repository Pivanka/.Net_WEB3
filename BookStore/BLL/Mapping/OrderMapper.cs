using AutoMapper;
using BLL.Dtos;
using DAL.Models;

namespace BLL.Mapping;

public class OrderMapper : Profile
{
    public OrderMapper()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<CartItem, OrderItem>();
        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.Author,
                opt => opt.MapFrom(x => x.Book.Author))
            .ForMember(dest => dest.Cover,
                opt => opt.MapFrom(x => x.Book.Cover))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(x => x.Book.Title))
            .ForMember(dest => dest.Price,
                opt => opt.MapFrom(x => x.Book.Price));
    }
}