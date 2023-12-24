using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Domain.Base;
using BookStore.Domain.Shared;

namespace BookStore.Domain.Entities;

public class Order : BaseEntity
{
    public List<OrderItem> Items { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public User? User;
    public int UserId { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}