using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using Bookstore.Infrastructure.Exceptions;
using BookStore.Infrastructure.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Application.CommandHandlers;

public record ClearCartCommand(int UserId) : IRequest;

public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand>
{
    private readonly IRepository<ShoppingCart> _shoppingCartRepository;

    public ClearCartCommandHandler(IRepository<ShoppingCart> shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }

    public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cartSpec = new ShoppingCartSpecification(request.UserId);
        var cart = await _shoppingCartRepository.Query(cartSpec).FirstOrDefaultAsync(cancellationToken);
        
        if (cart is null) throw new NotFoundException("Cart not found", ErrorType.NotFound);
        if (!cart.Items.Any()) throw new ArgumentNullException("Cart is empty");
        
        cart.Items.Clear();

        await _shoppingCartRepository.UpdateAsync(cart, cancellationToken);
        await _shoppingCartRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}