using AutoMapper;
using BLL.Dtos;
using DAL.Models;

namespace BLL.Mapping;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDTO>().ReverseMap();
    }
}