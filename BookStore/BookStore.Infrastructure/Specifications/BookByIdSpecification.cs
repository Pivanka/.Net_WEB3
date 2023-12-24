using Ardalis.Specification;
using BookStore.Domain.Entities;

namespace BookStore.Infrastructure.Specifications;

public class BookByIdSpecification : Specification<Book>
{
    public BookByIdSpecification(int bookId)
    {
        Query.Include(x => x.Reviews);
        Query.Include(x => x.Ratings);
        Query.Where(x => x.Id == bookId);
    }
}