using AutoMapper;
using BLL.Dtos;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.QueryHandlers;

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
        var shoppingCart = await _shoppingCartRepository
            .Query()
            .Include(x => x.Items)
            .ThenInclude(x => x.Book)
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

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