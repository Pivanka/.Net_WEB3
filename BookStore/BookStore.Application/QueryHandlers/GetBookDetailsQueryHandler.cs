using AutoMapper;
using Bookstore.Application.DTOs;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using Bookstore.Infrastructure.Exceptions;
using BookStore.Infrastructure.Specifications;
using MediatR;

namespace Bookstore.Application.QueryHandlers;

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
        var bookSpec = new BookByIdSpecification(request.BookId);
        var book = await _bookRepository.GetByIdAsync(bookSpec, cancellationToken);

        if (book is null) throw new NotFoundException("Book not found", ErrorType.NotFound);

        return _mapper.Map<BookDetails>(book);
    }
}