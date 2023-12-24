using BookStore.Domain.Entities;

namespace Bookstore.Application.DTOs;

public class ShoppingCartDto
{
    public List<CartItem> Items { get; set; }
}