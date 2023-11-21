using BLL.Dtos;

namespace BLL.Models;

public class AuthResponseModel
{
    public UserDTO User { get; set; }
    public string Token { get; set; }
}