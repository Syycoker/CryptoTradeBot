using System.Security.Cryptography;
using System.Text;
using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using Microsoft.Extensions.Logging;

namespace FractionalCryptoBot.Services
{
  public abstract class HttpService : IHttpService
  {
    /// <summary>
    /// The HttpClient for the service.
    /// </summary>
    public readonly HttpClient HttpClient;

    /// <summary>
    /// The Logger for the service.
    /// </summary>
    public readonly ILogger Log;

    /// <summary>
    /// The base uri for the service.
    /// </summary>
    public readonly string BaseUri;

    /// <summary>
    /// The api key to the client's service account.
    /// </summary>
    public readonly string ApiKey;

    /// <summary>
    /// The secret key to the client's service account.
    /// </summary>
    public readonly string ApiSecret;
    
    /// <summary>
    /// The default constructor to any class inherting from HttpService.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClient"></param>
    public HttpService(ILogger logger, HttpClient httpClient, Marketplaces marketplace)
    {
      // Use dependency Injection to set the client and logger from the 'top level'.
      HttpClient = httpClient;
      Log = logger;

      BaseUri = AuthenticationConfig.GetBaseUri(marketplace);
      ApiKey = AuthenticationConfig.GetApiKey(marketplace);
      ApiSecret = AuthenticationConfig.GetApiSecret(marketplace);
    }

    public abstract Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null);
    public abstract Task<string> SendPublicAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null);
    public abstract Task<string> SendSignedAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null);
    public virtual string Sign(string source, string key)
    {
      byte[] keyBytes = Encoding.UTF8.GetBytes(key);

      using (HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes))
      {
        byte[] sourceBytes = Encoding.UTF8.GetBytes(source);

        byte[] hash = hmacsha256.ComputeHash(sourceBytes);

        return BitConverter.ToString(hash).Replace("-", "").ToLower();
      }
    }
  }
}
