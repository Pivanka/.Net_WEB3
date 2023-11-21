namespace DAL.Models;

public class CartItem : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int Amount { get; set; }
}