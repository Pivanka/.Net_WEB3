using BLL.Exceptions;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.CommandHandlers;

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
        var cart = await _shoppingCartRepository.Query()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        
        if (cart is null) throw new NotFoundException("Cart not found", ErrorType.NotFound);
        if (!cart.Items.Any()) throw new ArgumentNullException("Cart is empty");
        
        cart.Items.Clear();

        await _shoppingCartRepository.UpdateAsync(cart, cancellationToken);
        await _shoppingCartRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}