using AutoMapper;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.CommandHandlers;

public record AddBookToCartCommand(int UserId, int BookId, int Count) : IRequest;

public class AddBookToCartCommandHandler : IRequestHandler<AddBookToCartCommand>
{
    private readonly IRepository<ShoppingCart> _shoppingCartRepository;
    private readonly IMapper _mapper;

    public AddBookToCartCommandHandler(IRepository<ShoppingCart> shoppingCartRepository, IMapper mapper)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddBookToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _shoppingCartRepository
            .Query()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        if (cart is null)
        {
            var newCart = _mapper.Map<ShoppingCart>(request);
            await _shoppingCartRepository.AddAsync(newCart, cancellationToken);
        }
        else
        {
            var newCartItem = _mapper.Map<CartItem>(request);
            cart.Items.Add(newCartItem);
            await _shoppingCartRepository.UpdateAsync(cart, cancellationToken);
        }
        
        await _shoppingCartRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}