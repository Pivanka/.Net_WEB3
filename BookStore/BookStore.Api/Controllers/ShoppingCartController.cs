using BookStore.Api.Filters;
using BookStore.Api.Models;
using Bookstore.Application.CommandHandlers;
using Bookstore.Application.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

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