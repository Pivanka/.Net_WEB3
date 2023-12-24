using AutoMapper;
using Bookstore.Application.CommandHandlers;
using Bookstore.Application.DTOs;
using BookStore.Domain.Entities;

namespace Bookstore.Application.Mapping;

public class ReviewMapper : Profile
{
    public ReviewMapper()
    {
        CreateMap<AddReviewCommand, Review>().ReverseMap();
        CreateMap<Review, ReviewDto>()
            .ForMember(dto => dto.Reviewer,
                opt => opt.MapFrom(x => x.User != null ? x.User.UserName : "Anonymous"));
    }
}

