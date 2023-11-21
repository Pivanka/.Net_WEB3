using BLL.CommandHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PL.WebApi.Filters;

namespace PL.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(ExceptionHandlingFilter))]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return BadRequest("Model is not valid");
            
        var result = await _mediator.Send(model, cancellationToken);
            
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return BadRequest("Some credentials are not valid");
            
        var result = await _mediator.Send(model, cancellationToken);

        return Ok(result);
    }
}