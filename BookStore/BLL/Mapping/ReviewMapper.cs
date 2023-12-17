using AutoMapper;
using BLL.CommandHandlers;
using BLL.Dtos;
using DAL.Models;

namespace BLL.Mapping;

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

