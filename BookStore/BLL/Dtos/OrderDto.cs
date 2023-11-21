using DAL.Models;

namespace BLL.Dtos;

public class OrderDto
{
    public List<OrderItem> Items { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}