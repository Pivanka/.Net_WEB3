using BookStore.Domain.Base;

namespace BookStore.Domain.Entities;

public class CartItem : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int Amount { get; set; }
}