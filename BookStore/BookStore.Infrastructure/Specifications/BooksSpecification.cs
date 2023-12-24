using Ardalis.Specification;
using BookStore.Domain.Entities;

namespace BookStore.Infrastructure.Specifications;

public class BooksSpecification : Specification<Book>
{
    public BooksSpecification()
    {
        Query.Include(x => x.Reviews);
        Query.Include(x => x.Ratings);
    }
}