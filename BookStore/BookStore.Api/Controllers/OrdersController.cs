using BookStore.Api.Filters;
using BookStore.Api.Models;
using Bookstore.Application.CommandHandlers;
using Bookstore.Application.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

[Route("api/orders")]
[ApiController]
[TypeFilter(typeof(ExceptionHandlingFilter))]
[Authorize]
public class OrdersController : BaseController
{
    private readonly IMediator _mediator;
    
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetOrdersQuery(UserId), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return BadRequest("Model is not valid");
            
        var result = await _mediator.Send(new CreateOrderCommand(UserId, model.ShippingAddress), cancellationToken);
        
        return Ok(result);
    }
}