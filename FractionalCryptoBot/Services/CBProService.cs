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
  /// The service class for Coinbase Pro.
  /// </summary>
  public sealed class CBProService : IHttpService
  {
    #region Private Members
    private HttpClient httpClient;
    private IAuthentication? authentication;
    private ILogger log;
    private string SignedString = "";
    #endregion
    #region Public Members
    public HttpClient Client { get => httpClient; private set => httpClient = value; }
    public ILogger Log { get => log; private set => log = value; }
    public string BaseUri => Authentication?.Uri ?? string.Empty;
    public string WebsocketBaseUri => Authentication?.WebsocketUri ?? string.Empty;
    public IAuthentication? Authentication { get => authentication; set => authentication = value; }
    #endregion
    #region Constructor
    /// <summary>
    /// Default constuctor to instantiate a Coinabse Pro service object.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClient"></param>
    public CBProService(ILogger logger)
    {
      // Nothing needs to be set in the constructor for now.
      httpClient = new HttpClient()
      {
        BaseAddress = new Uri(Authentication?.Uri ?? ""),  
      };

      authentication = AuthenticationConfig.GetAuthentication(Marketplaces.COINBASE_PRO);
      log = logger;

      // Not using string interpolation as I lose more valuable information and processing time when doing so.
      Log.LogInformation("{0}: '{1}' has been instantiated.", DateTime.UtcNow, nameof(CBProService));
    }
    #endregion
    #region Public Methods
    public async Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      using (var request = new HttpRequestMessage(httpMethod, BaseUri + requestUri))
      {
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("CB-ACCESS-KEY", Authentication?.Key);
        request.Headers.Add("CB-ACCESS-SIGN", SignedString);
        request.Headers.Add("CB-ACCESS-TIMESTAMP", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
        request.Headers.Add("CB-ACCESS-PASSPHRASE", Authentication?.Pass);

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

      string secret = Authentication?.Secret ?? string.Empty;

      var signature = Sign(queryStringBuilder.ToString(), secret);
      SignedString = signature;
      queryStringBuilder.Append("&signature=").Append(signature);

      StringBuilder requestUriBuilder = new StringBuilder(requestUri);
      requestUriBuilder.Append("?").Append(queryStringBuilder.ToString());

      return await SendAsync(httpMethod, requestUriBuilder.ToString(), content);
    }

    public void ParseWebsocketPayload(Crypto crypto, string content)
    {
      throw new NotImplementedException();
    }

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

    public string GetWebsocketPath(params string[] content)
    {
      return $"{Authentication?.WebsocketUri}";
    }
    #endregion
  }
}
