using System.Net.Http.Json;
using BLL.CommandHandlers;
using BLL.Dtos;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PL.WebApi.Models;
using Tests.IntegrationTests;
using Xunit;

namespace Tests.IntegrationTests;

public class OrdersTests : BaseTestFixture
{
    public OrdersTests(BookStoreWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task CreateOrder_Test()
    {
        //Arrange
        using var serviceScope = ServiceProvider.GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        var bookInCart = new AddBookToCartCommand(1, 1, 1);
        await mediator.Send(bookInCart);
        
        var request = new CreateOrderModel("Test address");

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/orders", request);

        //Assert
        response.EnsureSuccessStatusCode();
        var orderId = await response.Content.ReadFromJsonAsync<int>();
        orderId.Should().BeGreaterThan(0);

        var responseOrders = await HttpClient.GetAsync("api/orders");

        responseOrders.EnsureSuccessStatusCode();
        var orders = await responseOrders.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>();
        orders.Should().NotBeNullOrEmpty();
        orders!.Count().Should().Be(1);
        orders!.First().ShippingAddress.Should().BeEquivalentTo(request.ShippingAddress);
    }
}