using AutoMapper;
using BLL.Exceptions;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.CommandHandlers;

public record CreateOrderCommand(int UserId, string ShippingAddress) : IRequest<int>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<ShoppingCart> _shoppingCartRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(IRepository<Order> orderRepository, 
        IRepository<ShoppingCart> shoppingCartRepository, 
        IMapper mapper, 
        IMediator mediator)
    {
        _orderRepository = orderRepository;
        _shoppingCartRepository = shoppingCartRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await _shoppingCartRepository.Query()
            .Include(x => x.Items)
            .ThenInclude(x => x.Book)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        
        if (cart is null) throw new NotFoundException("Cart not found", ErrorType.NotFound);
        if (!cart.Items.Any()) throw new ArgumentNullException("Cart is empty");
        
        var order = new Order
        {
            UserId = request.UserId,
            Items = cart.Items.Select(x => new OrderItem
            {
                BookId = x.BookId,
                Amount = x.Amount
            }).ToList(),
            OrderDate = DateTime.Now,
            TotalPrice = cart.Items.Sum(x => x.Amount * x.Book.Price),
            ShippingAddress = request.ShippingAddress,
            Status = OrderStatus.Pending
        };

        var createdOrder = await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        var clearCartCommand = new ClearCartCommand(request.UserId);
        await _mediator.Send(clearCartCommand, cancellationToken);

        return createdOrder.Id;
    }
}
