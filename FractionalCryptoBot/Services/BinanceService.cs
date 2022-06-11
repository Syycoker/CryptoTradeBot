using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// The service class for binance.
  /// </summary>
  public sealed class BinanceService : IHttpService
  {
    #region Private Members
    private HttpClient httpClient;
    private IAuthentication? authentication;
    private ILogger log;
    private string baseUri = string.Empty;
    private string websocketBaseUri = string.Empty;
    private string klineStreamInterval = string.Empty;
    #endregion
    #region Public Members
    public HttpClient Client { get => httpClient; private set => httpClient = value; }
    public ILogger Log { get => log; private set => log = value; }
    public string BaseUri { get => baseUri; private set => baseUri = value; }
    public string WebsocketBaseUri { get => websocketBaseUri; private set => websocketBaseUri = value; }
    public string KlineStreamInterval { get => klineStreamInterval; private set => klineStreamInterval = value; }
    public IAuthentication? Authentication { get => authentication; private set => authentication = value; }
    #endregion
    #region Constructor
    /// <summary>
    /// Default constuctor to instantiate a binance service object.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClient"></param>
    public BinanceService(ILogger logger)
    {
      // Nothing needs to be set in the constructor for now.
      httpClient = new HttpClient();
      authentication = AuthenticationConfig.GetAuthentication(Marketplaces.BINANCE);
      log = logger;

      // Not using string interpolation as I lose more valuable information and processing time when doing so.
      Log.LogInformation("{0}: '{1}' has been instantiated.", DateTime.UtcNow, nameof(BinanceService));
    }
    #endregion
    #region Public Members
    /// <summary>
    /// Helper method to send an asynchoronus call to binance's endpoints.
    /// </summary>
    /// <param name="httpMethod">'GET', 'POST', 'DELETE'.</param>
    /// <param name="requestUri">The address of the endpoint.</param>
    /// <param name="content">The content to go with the call.</param>
    /// <returns>A response string in JSON.</returns>
    public async Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      using (var request = new HttpRequestMessage(httpMethod, BaseUri + requestUri))
      {
        request.Headers.Add("X-MBX-APIKEY", Authentication?.ApiKey);

        if (!(content is null))
          request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await Client.SendAsync(request);

        using (HttpContent responseContent = response.Content)
        {
          string jsonString = await responseContent.ReadAsStringAsync();

          return jsonString;
        }
      }
    }

    /// <summary>
    /// Makes a public call to Binance's Api Endpoints that doesn't need to specifiy a client's authorisation keys.
    /// </summary>
    /// <param name="httpMethod">'GET', 'POST', 'DELETE'.</param>
    /// <param name="requestUri">The address of the endpoint.</param>
    /// <param name="query">Queries to be added to the request uri.</param>
    /// <param name="content">The content to go with the call.</param>
    /// <returns>A response string in JSON.</returns>
    public async Task<string> SendPublicAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
    {
      if (!(query is null))
      {
        string queryString = string.Join("&", query.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value?.ToString())).Select(kvp => string.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(kvp.Value.ToString()))));

        if (!string.IsNullOrWhiteSpace(queryString))
        {
          requestUri += "?" + queryString;
        }
      }

      return await SendAsync(httpMethod, requestUri, content);
    }

    /// <summary>
    /// Makes a signed call to Binance's Api Endpoints that needs to specifiy a client's authorisation keys.
    /// </summary>
    /// <param name="httpMethod">'GET', 'POST', 'DELETE'.</param>
    /// <param name="requestUri">The address of the endpoint.</param>
    /// <param name="query">Queries to be added to the request uri.</param>
    /// <param name="content">The content to go with the call.</param>
    /// <returns>A response string in JSON.</returns>
    public async Task<string> SendSignedAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
    {
      StringBuilder queryStringBuilder = new StringBuilder();

      if (!(query is null))
      {
        string queryParameterString = string.Join("&", query.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value?.ToString())).Select(kvp => string.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(kvp.Value.ToString()))));
        queryStringBuilder.Append(queryParameterString);
      }

      if (queryStringBuilder.Length > 0)
        queryStringBuilder.Append("&");

      long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
      queryStringBuilder.Append("timestamp=").Append(now);

      string secret = Authentication?.ApiSecret ?? string.Empty;

      string signature = Sign(queryStringBuilder.ToString(), secret);
      queryStringBuilder.Append("&signature=").Append(signature);

      StringBuilder requestUriBuilder = new StringBuilder(requestUri);
      requestUriBuilder.Append("?").Append(queryStringBuilder.ToString());

      return await SendAsync(httpMethod, requestUriBuilder.ToString(), content);
    }

    /// <summary>
    /// Parses the payload.
    /// </summary>
    /// <param name="payload"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void ParseWebsocketPayload(Crypto crypto, string payload)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// HMAC signs the string if provided a source and its secret.
    /// </summary>
    /// <param name="source">The source string to then be encrypted.</param>
    /// <param name="key">The secret key to encrypt the source.</param>
    /// <returns>An encrypted string.</returns>
    public string Sign(string source, string key)
    {
      byte[] keyBytes = Encoding.UTF8.GetBytes(key);

      using (HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes))
      {
        byte[] sourceBytes = Encoding.UTF8.GetBytes(source);

        byte[] hash = hmacsha256.ComputeHash(sourceBytes);

        return BitConverter.ToString(hash).Replace("-", "").ToLower();
      }
    }

    public (string, string) GetWebsocketEndpoint()
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
