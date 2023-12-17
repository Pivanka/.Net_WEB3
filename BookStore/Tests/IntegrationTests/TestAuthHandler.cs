using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tests.IntegrationTests;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";
    private readonly BookStoreWebApplicationFactory.MockAuthUser _mockAuthUser;

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        BookStoreWebApplicationFactory.MockAuthUser mockAuthUser) : base(options, logger, encoder, clock)
    {
        _mockAuthUser = mockAuthUser;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_mockAuthUser.Claims.Count == 0)
            return Task.FromResult(AuthenticateResult.Fail("Mock auth user not configured."));

        var identity = new ClaimsIdentity(_mockAuthUser.Claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}