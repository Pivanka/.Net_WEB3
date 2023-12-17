using DAL.Models;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests;

public class RepositoryTests : IClassFixture<RepositorySeedDataFixture>
{
    private CancellationToken _ct = CancellationToken.None;
    private readonly Repository<Book> _bookRepository;

    public RepositoryTests(RepositorySeedDataFixture fixture)
    {
        _bookRepository = new Repository<Book>(fixture.BookStoreDbContext);
    }
        
    [Fact]
    public async Task Add_AddedEntity()
    {
        // Arrange
        using var fixture = new RepositorySeedDataFixture();
        var bookRepository = new Repository<Book>(fixture.BookStoreDbContext);
        
        var book = new Book
        {
            Title = "Test", 
            Author = "Test",
            Cover = "Test",
            Content = "Test",
            Genre = Genre.Novel,
            Price = 100
        };
        var expectedCount = fixture.BookStoreDbContext.Books.Count() + 1;

        // Act
        await bookRepository.AddAsync(book, _ct);
        await bookRepository.SaveChangesAsync(_ct);
        var actualCount = fixture.BookStoreDbContext.Books.Count();

        //Assert
        expectedCount.Should().Be(actualCount);
    }
    
    [Fact]
    public async Task DeleteEntity_Test()
    {
        // Arrange
        using var fixture = new RepositorySeedDataFixture();
        var bookRepository = new Repository<Book>(fixture.BookStoreDbContext);
        
        var books = fixture.BookStoreDbContext.Books;
        var expectedCount = books.Count() - 1;

        // Act
        await bookRepository.RemoveAsync(books.First(), _ct);
        await bookRepository.SaveChangesAsync(_ct);
        var actualCount = fixture.BookStoreDbContext.Books.Count();

        //Assert
        actualCount.Should().Be(expectedCount);
    }

    [Fact]
    public async Task FirstOrDefaultEntity_Not_Existing_Id()
    {
        // Arrange
        using var fixture = new RepositorySeedDataFixture();
        var bookRepository = new Repository<Book>(fixture.BookStoreDbContext);

        var notExistingId = -1;

        // Act
        var actualResult = await bookRepository.FirstOrDefaultAsync(x => x.Id == notExistingId, _ct);

        // Assert
        actualResult.Should().BeNull();
    }

    [Fact]
    public async Task FirstOrDefaultEntity_Existing_Id()
    {
        // Arrange
        using var fixture = new RepositorySeedDataFixture();
        var bookRepository = new Repository<Book>(fixture.BookStoreDbContext);

        var expectedBook = fixture.BookStoreDbContext.Books.First();

        // Act
        var actualBook = await bookRepository.FirstOrDefaultAsync(x => x.Id == expectedBook.Id, _ct);

        // Assert
        expectedBook.Id.Should().Be(actualBook!.Id);
    }

    [Fact]
    public async Task UpdateEntity_Not_Null()
    {
        // Arrange
        using var fixture = new RepositorySeedDataFixture();
        var bookRepository = new Repository<Book>(fixture.BookStoreDbContext);

        var book = fixture.BookStoreDbContext.Books.First();
        var expectedTitle= "Test 2";
        book.Title = expectedTitle;

        // Act
        await bookRepository.UpdateAsync(book, _ct);
        await bookRepository.SaveChangesAsync(_ct);
        var actualTitle = (await fixture.BookStoreDbContext.Books.FirstOrDefaultAsync(x => x.Id == book.Id, _ct))?.Title;

        // Assert
        actualTitle.Should().BeEquivalentTo(expectedTitle);
    }
}