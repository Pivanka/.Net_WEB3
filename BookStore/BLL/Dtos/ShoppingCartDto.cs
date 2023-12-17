using DAL.Models;

namespace BLL.Dtos;

public class ShoppingCartDto
{
    public List<CartItem> Items { get; set; }
}