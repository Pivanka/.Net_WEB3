using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests;

public class RepositorySeedDataFixture: IDisposable
{
    public RepositorySeedDataFixture()
    {
        BookStoreDbContext.Books.Add(new Book
        {
            Id = 1, Title = "1984", Author = "Goerge Orwell",
            Cover =
                "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1327144697l/3744438.jpg",
            Content =
                "1984 is a dystopian novella by George Orwell published in 1949, which follows the life of Winston Smith, a low ranking member of 'the Party', who is frustrated by the omnipresent eyes of the party, and its ominous ruler Big Brother. 'Big Brother' controls every aspect of people's lives.",
            Genre = Genre.Novel,
            Price = 100
        });
        BookStoreDbContext.Books.Add(new Book
        {
            Id = 2, Title = "The Little Prince", Author = "Antoine de Saint-Exupéry",
            Cover = "https://images-na.ssl-images-amazon.com/images/I/71OZY035QKL.jpg",
            Content =
                "The Little Prince is an honest and beautiful story about loneliness, friendship, sadness, and love. The prince is a small boy from a tiny planet (an asteroid to be precise), who travels the universe, planet-to-planet, seeking wisdom. On his journey, he discovers the unpredictable nature of adults.",
            Genre = Genre.Novella,
            Price = 110
        });
        BookStoreDbContext.SaveChanges();
    }

    public BookStoreDbContext BookStoreDbContext { get; } = new BookStoreDbContext(
        new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    public void Dispose()
    {
        BookStoreDbContext.Database.EnsureDeleted();
        BookStoreDbContext.Dispose();
    }
}