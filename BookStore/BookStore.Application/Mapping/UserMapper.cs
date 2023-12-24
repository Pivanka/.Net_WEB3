using AutoMapper;
using Bookstore.Application.DTOs;
using BookStore.Domain.Entities;

namespace Bookstore.Application.Mapping;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDTO>().ReverseMap();
    }
}