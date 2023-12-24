using BookStore.Api.Filters;
using BookStore.Api.Models;
using Bookstore.Application.CommandHandlers;
using Bookstore.Application.DTOs;
using Bookstore.Application.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(ExceptionHandlingFilter))]
public class BooksController : BaseController
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromBody] GetBooksQuery model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return BadRequest();
            
        var books = await _mediator.Send(model, cancellationToken);
            
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDetails>> GetBookDetails(int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetBookDetailsQuery(id), cancellationToken));
    }
        
    [HttpPost("review")]
    [Authorize]
    public async Task<IActionResult> Review([FromBody] AddReviewModel model, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddReviewCommand(model.BookId, UserId, model.Message), cancellationToken);
            
        return Ok();
    }

    [HttpPost("rate")]
    [Authorize]
    public async Task<ActionResult<int>> Rate([FromBody] AddRatingModel model, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddRatingCommand(UserId, model.BookId, model.Score), cancellationToken);
            
        return Ok();
    }
}