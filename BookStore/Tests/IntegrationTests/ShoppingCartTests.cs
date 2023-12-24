using System.Net.Http.Json;
using BookStore.Api.Models;
using Bookstore.Application.CommandHandlers;
using Bookstore.Application.DTOs;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.IntegrationTests;

public class ShoppingCartTests : BaseTestFixture
{
    public ShoppingCartTests(BookStoreWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task AddBookToCart_Test()
    {
        //Arrange
        var request = new AddBookToCartModel(1, 1);

        //Act
        var response = await HttpClient.PostAsJsonAsync("api/ShoppingCart", request);

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetShoppingCart_Test()
    {
        //Arrange
        using var serviceScope = ServiceProvider.GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        var bookInCart = new AddBookToCartCommand(1, 1, 1);
        await mediator.Send(bookInCart);

        //Act
        var response = await HttpClient.GetAsync("api/ShoppingCart");

        //Assert
        response.EnsureSuccessStatusCode();
        var cart = await response.Content.ReadFromJsonAsync<ShoppingCartDto>();
        cart.Should().NotBeNull();
        cart!.Items.First().BookId.Should().Be(bookInCart.BookId);
    }
}