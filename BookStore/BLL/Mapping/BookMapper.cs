using AutoMapper;
using BLL.CommandHandlers;
using BLL.Dtos;
using DAL.Models;

namespace BLL.Mapping;

internal class BookMapper : Profile
{
    public BookMapper()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dto => dto.ReviewsNumber,
            opt => opt.MapFrom(book => book.Reviews.Count))
            .ForMember(dto => dto.Rating,
            opt => opt.MapFrom(book => book.Ratings.Count != 0 ? Math.Round(book.Ratings.Average(r => r.Score), 2) : 0))
            .ReverseMap();

        CreateMap<Book, BookDetails>()
            .ForMember(dto => dto.Reviews,
            opt => opt.MapFrom(book => book.Reviews))
            .ForMember(dto => dto.Rating,
            opt => opt.MapFrom(book => book.Ratings.Count != 0 ? Math.Round(book.Ratings.Average(r => r.Score), 2) : 0))
            .ReverseMap();

        CreateMap<AddBookToCartCommand, ShoppingCart>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(x => x.UserId))
            .ForMember(dest => dest.Items,
                opt => opt.MapFrom(x => new List<CartItem>{ new() { BookId = x.BookId, Amount = x.Count } }));

        CreateMap<AddBookToCartCommand, CartItem>()
            .ForMember(dest => dest.Amount,
                opt => opt.MapFrom(x => x.Count));
    }
}

