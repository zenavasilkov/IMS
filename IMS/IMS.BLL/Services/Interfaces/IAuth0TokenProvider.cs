namespace IMS.BLL.Services.Interfaces;

public interface IAuth0TokenProvider
{
    Task<string> GetTokenAsync();
}
