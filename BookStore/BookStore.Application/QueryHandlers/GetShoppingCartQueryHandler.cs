using AutoMapper;
using Bookstore.Application.DTOs;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Application.QueryHandlers;

public record GetShoppingCartQuery(int UserId) : IRequest<ShoppingCartDto>;

public class GetShoppingCartQueryHandler : IRequestHandler<GetShoppingCartQuery, ShoppingCartDto>
{
    private readonly IRepository<ShoppingCart> _shoppingCartRepository;
    private readonly IMapper _mapper;

    public GetShoppingCartQueryHandler(IMapper mapper, IRepository<ShoppingCart> shoppingCartRepository)
    {
        _mapper = mapper;
        _shoppingCartRepository = shoppingCartRepository;
    }

    public async Task<ShoppingCartDto> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
    {
        var cartSpec = new ShoppingCartSpecification(request.UserId);
        var shoppingCart = await _shoppingCartRepository.Query(cartSpec).FirstOrDefaultAsync(cancellationToken);

        if (shoppingCart is null)
        {
            var newCart = new ShoppingCart
            {
                UserId = request.UserId
            };
            
            shoppingCart = await _shoppingCartRepository.AddAsync(newCart, cancellationToken);
            await _shoppingCartRepository.SaveChangesAsync(cancellationToken);
        }
        
        return _mapper.Map<ShoppingCartDto>(shoppingCart);
    }
}