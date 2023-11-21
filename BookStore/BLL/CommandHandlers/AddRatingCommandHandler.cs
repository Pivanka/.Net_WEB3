using AutoMapper;
using BLL.Exceptions;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;

namespace BLL.CommandHandlers;

public record AddRatingCommand(int UserId, int BookId, int Score) : IRequest;

public class AddRatingCommandHandler : IRequestHandler<AddRatingCommand>
{
    private readonly IRepository<Rating> _ratingRepository;
    private readonly IRepository<Book> _bookRepository;
    private readonly IMapper _mapper;

    public AddRatingCommandHandler(IRepository<Rating> ratingRepository, 
        IMapper mapper, 
        IRepository<Book> bookRepository)
    {
        _ratingRepository = ratingRepository;
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<Unit> Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FirstOrDefaultAsync(x => x.Id == request.BookId, cancellationToken);
        
        if (book is null) throw new NotFoundException("Book not found", ErrorType.NotFound);
        
        var ratingToAdd = _mapper.Map<Rating>(request);

        await _ratingRepository.AddAsync(ratingToAdd, cancellationToken);
        await _ratingRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}