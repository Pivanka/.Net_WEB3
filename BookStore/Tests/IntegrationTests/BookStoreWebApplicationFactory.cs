using System.Security.Claims;
using DAL.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Tests.IntegrationTests;

public class BookStoreWebApplicationFactory : WebApplicationFactory<Program>
{
    private int DefaultUserId { get; set; } = 1;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<BookStoreDbContext>));

            services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDB");
            });
            
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(TestAuthHandler.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme, options => { });
            services.AddScoped(_ => _user);   

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService
                <ILogger<BookStoreWebApplicationFactory>>();
            db.Database.EnsureCreated();

            try
            {
                db.InitializeTestDatabase();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $@"An error occurred seeding the database 
                                    with test messages. Error: {ex.Message}");
            }
        });
    }
    
    private readonly MockAuthUser _user = new(new Claim(ClaimTypes.NameIdentifier, 1.ToString()));
    
    public class MockAuthUser
    {
        public List<Claim> Claims { get; private set; } = new();

        public MockAuthUser(params Claim[] claims)
            => Claims = claims.ToList();
    }
}