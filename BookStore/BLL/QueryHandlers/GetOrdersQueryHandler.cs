using AutoMapper;
using BLL.Dtos;
using DAL.Models;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.QueryHandlers;

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
        var orders = await _orderRepository
            .Query()
            .Include(x => x.Items)
            .ThenInclude(x => x.Book)
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}