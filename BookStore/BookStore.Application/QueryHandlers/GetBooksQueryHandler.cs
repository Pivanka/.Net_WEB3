using AutoMapper;
using Bookstore.Application.DTOs;
using Bookstore.Application.Models;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Shared;
using BookStore.Infrastructure.Specifications;
using MediatR;

namespace Bookstore.Application.QueryHandlers;

public record GetBooksQuery : IRequest<IEnumerable<BookDto>>
{
    public Genre? Category { get; set; }
    public SortOrder SortOrder { get; set; }
    public string SearchString { get; set; } = string.Empty;
    public bool IsDescending { get; set; }
}

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(IRepository<Book> bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync(new BooksSpecification(), cancellationToken);

        if (request.Category is not null)
        {
            books = books.Where(x => x.Genre == request.Category);
        }
        
        if (!string.IsNullOrEmpty(request.SearchString))
            books = books.Where(x => x.Author.ToLower().Contains(request.SearchString.ToLower())
                                     || x.Title.ToLower().Contains(request.SearchString.ToLower()));

        books = request.SortOrder switch
        {
            SortOrder.Author => request.IsDescending 
                ? books.OrderByDescending(b => b.Author) 
                : books.OrderBy(b => b.Author),
            SortOrder.Title => request.IsDescending 
                ? books.OrderByDescending(o => o.Title)
                : books.OrderBy(o => o.Title),
            SortOrder.Genre => request.IsDescending 
                ? books.OrderByDescending(o => o.Genre)
                : books.OrderBy(o => o.Genre),
            SortOrder.Price => request.IsDescending 
                ? books.OrderByDescending(o => o.Price)
                : books.OrderBy(o => o.Price),
            _ =>  request.IsDescending 
                ? books.OrderByDescending(o => o.Id)
                : books.OrderBy(o => o.Id)
        };

        return _mapper.Map<IEnumerable<BookDto>>(books);
    }
}