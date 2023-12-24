using Bookstore.Application.DTOs;

namespace Bookstore.Application.Models;

public class AuthResponseModel
{
    public UserDTO User { get; set; }
    public string Token { get; set; }
}