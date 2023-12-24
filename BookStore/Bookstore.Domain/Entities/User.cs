using Microsoft.AspNetCore.Identity;

namespace BookStore.Domain.Entities;

public class User : IdentityUser<int>
{
    public List<Order> Orders { get; set; }
}