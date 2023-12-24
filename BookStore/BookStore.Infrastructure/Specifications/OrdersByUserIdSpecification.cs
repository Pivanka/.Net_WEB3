using Ardalis.Specification;
using BookStore.Domain.Entities;

namespace BookStore.Infrastructure.Specifications;

public class OrdersByUserIdSpecification : Specification<Order>
{
    public OrdersByUserIdSpecification(int userId)
    {
        Query.Include(x => x.Items).ThenInclude(x => x.Book);
        Query.Where(x => x.UserId == userId);
    }
}