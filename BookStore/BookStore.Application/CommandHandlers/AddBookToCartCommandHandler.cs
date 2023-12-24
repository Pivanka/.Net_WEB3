using AutoMapper;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Application.CommandHandlers;

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
        var cartSpec = new ShoppingCartSpecification(request.UserId);
        var cart = await _shoppingCartRepository.Query(cartSpec).FirstOrDefaultAsync(cancellationToken);

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