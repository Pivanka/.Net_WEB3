using AutoMapper;
using BLL.Dtos;
using BLL.Exceptions;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.QueryHandlers;

public record GetBookDetailsQuery(int BookId) : IRequest<BookDetails>;

public class GetBookDetailsQueryHandler : IRequestHandler<GetBookDetailsQuery, BookDetails>
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IMapper _mapper;

    public GetBookDetailsQueryHandler(IRepository<Book> bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }
    
    public async Task<BookDetails> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository
            .Query()
            .Include(b => b.Ratings)
            .Include(b => b.Reviews)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(b => b.Id == request.BookId, cancellationToken);

        if (book is null) throw new NotFoundException("Book not found", ErrorType.NotFound);

        return _mapper.Map<BookDetails>(book);
    }
}