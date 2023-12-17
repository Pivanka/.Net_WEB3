using System.Net.Http.Json;
using BLL.CommandHandlers;
using BLL.Models;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTests;

public class AuthControllerTests : BaseTestFixture
{
    public AuthControllerTests(BookStoreWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task RegisterUser_Test()
    {
        var request = new RegisterCommand
        {
            UserName = "test",
            Email = "test@test.com",
            Password = "Test12!",
            ConfirmPassword = "Test12!"
        };
        
        var response = await HttpClient.PostAsJsonAsync("api/auth/register", request);

        response.EnsureSuccessStatusCode();
        var registeredUser = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
        registeredUser.Should().NotBeNull();
        registeredUser!.Token.Should().NotBeNullOrEmpty();
        registeredUser!.User.Should().NotBeNull();
        registeredUser!.User.UserName.Should().BeEquivalentTo(request.UserName);
        registeredUser!.User.Email.Should().BeEquivalentTo(request.Email);
            
        var loginRequest = new LoginCommand(request.Email, request.Password);
        
        var loginResponse = await HttpClient.PostAsJsonAsync("api/auth/login", loginRequest);

        loginResponse.EnsureSuccessStatusCode();
    }
}