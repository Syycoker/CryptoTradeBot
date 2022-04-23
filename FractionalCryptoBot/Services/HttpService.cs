using System.Security.Cryptography;
using System.Text;
using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using Microsoft.Extensions.Logging;
using System.ServiceModel.Channels;

namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// An abstract class that implements 'IHttpService'.
  /// </summary>
  public abstract class HttpService : IHttpService
  {
    #region Public Members
    /// <summary>
    /// The HttpClient for the service.
    /// </summary>
    public readonly HttpClient Client;

    /// <summary>
    /// The Logger for the service.
    /// </summary>
    public readonly ILogger Log;

    /// <summary>
    /// The base uri for the service.
    /// </summary>
    public readonly string BaseUri;

    /// <summary>
    /// The base uri for the service's websocket.
    /// </summary>
    public readonly string WebsocketBaseUri;

    /// <summary>
    /// The api key to the client's service account.
    /// </summary>
    public readonly string ApiKey;

    /// <summary>
    /// The secret key to the client's service account.
    /// </summary>
    public readonly string ApiSecret;
    #endregion
    #region Constructor
    /// <summary>
    /// The default constructor to any class inherting from HttpService.
    /// </summary>
    /// <param name="logger">Any logger thn implements ILogger.</param>
    /// <param name="httpClient">The http client to be used to make calls.</param>
    public HttpService(ILogger logger, Marketplaces marketplace)
    {
      // Use dependency Injection to set the client and logger from the 'top level'.
      Client = new HttpClient();
      Log = logger;

      // Set the dependencies based on what service the user requests, cannot be modified once set, else re-instantiation of the object itself.
      BaseUri = AuthenticationConfig.GetBaseUri(marketplace);
      ApiKey = AuthenticationConfig.GetApiKey(marketplace);
      ApiSecret = AuthenticationConfig.GetApiSecret(marketplace);
      WebsocketBaseUri = AuthenticationConfig.GetWebsocketUri(marketplace);
    }
    #endregion
    #region Public Methods
    public abstract Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null);
    public abstract Task<string> SendPublicAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null);
    public abstract Task<string> SendSignedAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null);
    public abstract void SendWebsocketAsync(string parameter);
    public abstract void SocketOnClose(object? sender, EventArgs args);
    public abstract void SocketOnMessage(object? sender, EventArgs args);
    public abstract void SocketOnOpen(object? sender, EventArgs args);
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
    #endregion
  }
}