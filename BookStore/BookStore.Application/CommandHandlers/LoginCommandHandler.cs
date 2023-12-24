using AutoMapper;
using Bookstore.Application.DTOs;
using Bookstore.Application.Models;
using Bookstore.Application.Services.Contracts;
using BookStore.Domain.Entities;
using Bookstore.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Application.CommandHandlers;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseModel>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseModel>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenManager _tokenManager;
    private readonly IMapper _mapper;

    public LoginCommandHandler(UserManager<User> userManager,
        ITokenManager tokenManager, 
        IMapper mapper)
    {
        _userManager = userManager;
        _tokenManager = tokenManager;
        _mapper = mapper;
    }
    
    public async Task<AuthResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return new AuthResponseModel { User = new UserDTO(), Token = string.Empty};

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        
        if (!isPasswordCorrect)
            throw new InvalidUserCredentialsException("Password is not correct", ErrorType.InvalidPassword);

        var token = _tokenManager.GenerateToken(user);

        return new AuthResponseModel { User = _mapper.Map<UserDTO>(user), Token = token };
    }
}