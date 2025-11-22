using System.Security.Claims;
using System.Text.Encodings.Web;
using IMS.Presentation.ApiConstants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IMS.IntegrationTests.Helpers;

public class TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string ClaimType = "permissions";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),

            new Claim(ClaimType, Permissions.Users.Read),
            new Claim(ClaimType, Permissions.Users.Create),
            new Claim(ClaimType, Permissions.Users.Update),

            new Claim(ClaimType, Permissions.Boards.Read),
            new Claim(ClaimType, Permissions.Boards.Create),
            new Claim(ClaimType, Permissions.Boards.Update),

            new Claim(ClaimType, Permissions.Internships.Read),
            new Claim(ClaimType, Permissions.Internships.Create),
            new Claim(ClaimType, Permissions.Internships.Update),

            new Claim(ClaimType, Permissions.Tickets.Read),
            new Claim(ClaimType, Permissions.Tickets.Create),
            new Claim(ClaimType, Permissions.Tickets.Update),

            new Claim(ClaimType, Permissions.Feedbacks.Read),
            new Claim(ClaimType, Permissions.Feedbacks.Create),
            new Claim(ClaimType, Permissions.Feedbacks.Update),
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
