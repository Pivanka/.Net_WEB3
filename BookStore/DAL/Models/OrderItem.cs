namespace DAL.Models;

public class OrderItem : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int Amount { get; set; }
}