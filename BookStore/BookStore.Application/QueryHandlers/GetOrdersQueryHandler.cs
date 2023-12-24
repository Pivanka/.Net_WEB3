using AutoMapper;
using Bookstore.Application.DTOs;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Specifications;
using MediatR;

namespace Bookstore.Application.QueryHandlers;

public record GetOrdersQuery(int UserId) : IRequest<IEnumerable<OrderDto>>;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IRepository<Order> orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var ordersSpec = new OrdersByUserIdSpecification(request.UserId);
        var orders = await _orderRepository.GetAllAsync(ordersSpec, cancellationToken);
        
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}