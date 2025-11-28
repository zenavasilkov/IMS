using System.Text.Json;
using IMS.BLL.Exceptions;
using IMS.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace IMS.BLL.Services;

public class Auth0TokenProvider(IConfiguration config) : IAuth0TokenProvider
{
    private readonly string _domain = config["Auth0:Domain"] ?? 
        throw new  MissingConfigurationException("Missing 'Auth0:Domain' property in configurations");
    
    private readonly string _clientId = config["Auth0:ManagementClientId"] ??
        throw new  MissingConfigurationException("Missing 'Auth0:ManagementClientId' property in configurations");
    
    private readonly string _clientSecret = config["Auth0:ManagementClientSecret"] ??
        throw new MissingConfigurationException("Missing 'Auth0:ManagementClientSecret' property in configurations");
    
    private string _accessToken = "";
    private DateTime _expiresAt;

    private const string GrandType = "client_credentials";
    private const string AccessTokenProperty =  "access_token";
    private const int ExpiresInSeconds = 3600;

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _expiresAt) return _accessToken;

        using var client = new RestClient($"https://{_domain}/oauth/token");
        
        var request = new RestRequest("", Method.Post);

        request.AddHeader("content-type", "application/json");
        
        var body = new
        {
            grant_type = GrandType,
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
        
        _accessToken = doc.RootElement.GetProperty(AccessTokenProperty).GetString() ?? "";
        _expiresAt = DateTime.UtcNow.AddSeconds(ExpiresInSeconds);
        
        return _accessToken;
    }
}
