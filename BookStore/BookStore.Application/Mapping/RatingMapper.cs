using AutoMapper;
using Bookstore.Application.CommandHandlers;
using BookStore.Domain.Entities;

namespace Bookstore.Application.Mapping;

public class RatingMapper : Profile
{
    public RatingMapper()
    {
        CreateMap<AddRatingCommand, Rating>().ReverseMap();
    }
}

