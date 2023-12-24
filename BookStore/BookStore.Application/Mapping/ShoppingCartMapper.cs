using AutoMapper;
using Bookstore.Application.DTOs;
using BookStore.Domain.Entities;

namespace Bookstore.Application.Mapping;

public class ShoppingCartMapper : Profile
{
    public ShoppingCartMapper()
    {
        CreateMap<ShoppingCart, ShoppingCartDto>();
    }
}