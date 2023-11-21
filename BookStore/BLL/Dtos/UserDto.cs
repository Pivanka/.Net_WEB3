using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos;

public class UserDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
}