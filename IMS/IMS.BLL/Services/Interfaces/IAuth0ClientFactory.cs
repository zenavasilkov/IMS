using Auth0.ManagementApi;

namespace IMS.BLL.Services.Interfaces;

public interface IAuth0ClientFactory
{
    Task<ManagementApiClient> CreateClientAsync();
}
