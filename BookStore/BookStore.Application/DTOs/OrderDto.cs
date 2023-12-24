using BookStore.Domain.Entities;
using BookStore.Domain.Shared;

namespace Bookstore.Application.DTOs;

public class OrderDto
{
    public List<OrderItem> Items { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}