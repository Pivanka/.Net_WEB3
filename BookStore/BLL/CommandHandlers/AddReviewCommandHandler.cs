using AutoMapper;
using BLL.Exceptions;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;

namespace BLL.CommandHandlers;

public record AddReviewCommand(int BookId, int UserId, string Message) : IRequest;

public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand>
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Book> _bookRepository;

    public AddReviewCommandHandler(IMapper mapper, 
        IRepository<Review> reviewRepository,
        IRepository<Book> bookRepository)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
    }

    public async Task<Unit> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FirstOrDefaultAsync(x => x.Id == request.BookId, cancellationToken);
        
        if (book is null) throw new NotFoundException("Book not found", ErrorType.NotFound);

        var reviewToAdd = _mapper.Map<Review>(request);

        await _reviewRepository.AddAsync(reviewToAdd, cancellationToken);
        await _reviewRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}