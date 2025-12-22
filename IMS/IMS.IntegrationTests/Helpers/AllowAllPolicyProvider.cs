using Microsoft.AspNetCore.Authorization;

namespace IMS.IntegrationTests.Helpers;

public class AllowAllPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes("Test")
            .RequireAssertion(_ => true)
            .Build());
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        return Task.FromResult<AuthorizationPolicy?>(
            new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes("Test")
                .RequireAssertion(_ => true)
                .Build());
    }
}