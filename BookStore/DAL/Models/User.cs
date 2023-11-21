using Microsoft.AspNetCore.Identity;

namespace DAL.Models;

public class User : IdentityUser<int>
{
    public List<Order> Orders { get; set; }
}