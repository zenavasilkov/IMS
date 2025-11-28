using Auth0.ManagementApi;
using IMS.BLL.Exceptions;
using IMS.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IMS.BLL.Services;

public class Auth0ClientFactory(IAuth0TokenProvider tokenProvider, IConfiguration config) : IAuth0ClientFactory
{ 
    private readonly string _domain = config["Auth0:Domain"] ??
        throw new MissingConfigurationException("Missing 'Auth0:Domain' property in configurations'");

    public async Task<ManagementApiClient> CreateClientAsync()
    {
        var token = await tokenProvider.GetTokenAsync();
        return new ManagementApiClient(token, new Uri($"https://{_domain}/api/v2/"));
    }
}
