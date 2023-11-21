using BLL.CommandHandlers;
using BLL.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.WebApi.Filters;
using PL.WebApi.Models;

namespace PL.WebApi.Controllers;

[Route("api/[controller]")]
[TypeFilter(typeof(ExceptionHandlingFilter))]
[ApiController]
[Authorize]
public class ShoppingCartController : BaseController
{
    private readonly IMediator _mediator;

    public ShoppingCartController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShoppingCartQuery(UserId), cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBookToCart([FromBody] AddBookToCartModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return BadRequest("Model is not valid");

        await _mediator.Send(new AddBookToCartCommand(UserId, model.BookId, model.Count), cancellationToken);
        
        return Ok();
    }
}