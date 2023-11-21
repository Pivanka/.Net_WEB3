using BLL.CommandHandlers;
using BLL.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.WebApi.Filters;
using PL.WebApi.Models;

namespace PL.WebApi.Controllers;

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