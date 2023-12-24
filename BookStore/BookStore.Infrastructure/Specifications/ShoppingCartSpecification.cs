using Ardalis.Specification;
using BookStore.Domain.Entities;

namespace BookStore.Infrastructure.Specifications;

public class ShoppingCartSpecification : Specification<ShoppingCart>
{
    public ShoppingCartSpecification(int userId)
    {
        Query.Include(x => x.Items).ThenInclude(x => x.Book);
        Query.Where(x => x.UserId == userId);
    }
}