using System.Text.Json;
using IMS.BLL.Exceptions;
using IMS.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace IMS.BLL.Services;

public class Auth0TokenProvider(IConfiguration config) : IAuth0TokenProvider
{
    private readonly string _domain = config["Auth0:Domain"] ?? "";
    private readonly string _clientId = config["Auth0:ManagementClientId"] ?? "";
    private readonly string _clientSecret = config["Auth0:ManagementClientSecret"] ?? "";
    private string _accessToken = "";
    private DateTime _expiresAt;

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _expiresAt) return _accessToken;

        using var client = new RestClient($"https://{_domain}/oauth/token");
        
        var request = new RestRequest("", Method.Post);

        request.AddHeader("content-type", "application/json");
        
        var body = new
        {
            grant_type = "client_credentials",
            client_id = _clientId,
            client_secret = _clientSecret,
            audience = $"https://{_domain}/api/v2/"
        };
        
        request.AddJsonBody(body);
        
        var response = await client.ExecuteAsync(request);
        
        if (!response.IsSuccessful)
            throw new Auth0TokenRequestFailedException($"Auth0 token request failed: {response.StatusCode} {response.Content}");
        
        if (response.Content is null) 
            throw new Auth0TokenRequestFailedException($"Auth0 token request failed: response content is null");
        
        using var doc = JsonDocument.Parse(response.Content);
        
        _accessToken = doc.RootElement.GetProperty("access_token").GetString() ?? "";
        _expiresAt = DateTime.UtcNow.AddSeconds(3600);
        
        return _accessToken;
    }
}
