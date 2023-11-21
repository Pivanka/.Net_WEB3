using AutoMapper;
using BLL.Dtos;
using BLL.Exceptions;
using BLL.Models;
using BLL.Services.Contracts;
using DAL.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BLL.CommandHandlers;

public class RegisterCommand : IRequest<AuthResponseModel>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseModel>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly ITokenManager _tokenManager;

    public RegisterCommandHandler(UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IMapper mapper, 
        ITokenManager tokenManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _tokenManager = tokenManager;
    }
    
    public async Task<AuthResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (_userManager.FindByEmailAsync(request.Email).Result != null)
            throw new InvalidUserCredentialsException("User with this email already registered!", ErrorType.UserExists);;

        if (request.Password != request.ConfirmPassword)
            throw new InvalidUserCredentialsException("Passwords don't match", ErrorType.InvalidPassword);

        var userToAdd = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(userToAdd, request.Password);
        if (!result.Succeeded) throw new Exception(result.ToString());

        var user = await _userManager.FindByEmailAsync(userToAdd.Email);

        return new AuthResponseModel { User = _mapper.Map<UserDTO>(user), Token = _tokenManager.GenerateToken(user) };
    }
}