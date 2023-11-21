using AutoMapper;
using BLL.Dtos;
using DAL.Models;

namespace BLL.Mapping;

public class ShoppingCartMapper : Profile
{
    public ShoppingCartMapper()
    {
        CreateMap<ShoppingCart, ShoppingCartDto>();
    }
}