using BookStore.Domain.Base;

namespace BookStore.Domain.Entities;

public class ShoppingCart : BaseEntity
{
    public List<CartItem> Items { get; set; }
    public User? User { get; set; }
    public int UserId { get; set; }
}