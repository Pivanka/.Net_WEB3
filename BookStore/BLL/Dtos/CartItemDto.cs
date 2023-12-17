namespace BLL.Dtos;

public class CartItemDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Cover { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
}