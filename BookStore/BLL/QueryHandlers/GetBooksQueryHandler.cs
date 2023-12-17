using AutoMapper;
using BLL.Dtos;
using BLL.Models;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.QueryHandlers;

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
        var books = _bookRepository
            .Query()
            .Include(b => b.Ratings)
            .Include(b => b.Reviews)
            .AsQueryable();

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

        var result = await books.ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<BookDto>>(result);
    }
}