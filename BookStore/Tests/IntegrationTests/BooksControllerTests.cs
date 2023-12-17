using System.Net.Http.Json;
using BLL.Dtos;
using BLL.Models;
using BLL.QueryHandlers;
using FluentAssertions;
using PL.WebApi.Models;
using Xunit;

namespace Tests.IntegrationTests;

public class BooksControllerTests : BaseTestFixture
{
    public BooksControllerTests(BookStoreWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetBooks_Test()
    {
        var request = new GetBooksQuery
        {
            SortOrder = SortOrder.Price,
            IsDescending = true
        };
        
        var response = await HttpClient.PostAsJsonAsync("api/books", request);

        response.EnsureSuccessStatusCode();
        var books = await response.Content.ReadFromJsonAsync<IEnumerable<BookDto>>();
        books.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task RateBook_Test()
    {
        var request = new AddRatingModel(1, 2);
        
        var response = await HttpClient.PostAsJsonAsync("api/books/rate", request);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task ReviewBook_Test()
    {
        var request = new AddReviewModel(1, "Test");
        
        var response = await HttpClient.PostAsJsonAsync("api/books/review", request);

        response.EnsureSuccessStatusCode();
    }
}