using AutoMapper;
using BLL.CommandHandlers;
using DAL.Models;

namespace BLL.Mapping;

public class RatingMapper : Profile
{
    public RatingMapper()
    {
        CreateMap<AddRatingCommand, Rating>().ReverseMap();
    }
}

