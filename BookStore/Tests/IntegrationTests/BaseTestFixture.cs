using System.Net.Http.Headers;
using Xunit;

namespace Tests.IntegrationTests;

public class BaseTestFixture : IClassFixture<BookStoreWebApplicationFactory>
{
    protected readonly HttpClient HttpClient;
    protected readonly IServiceProvider ServiceProvider;
    
    public BaseTestFixture(BookStoreWebApplicationFactory factory)
    {
        HttpClient = factory.CreateClient();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        ServiceProvider = factory.Services;
    }
}