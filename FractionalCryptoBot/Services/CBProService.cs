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
  public sealed class CBProService : IHttpService
  {
    #region Private Members
    private HttpClient httpClient;
    private IAuthentication? authentication;
    private ILogger log;
    #endregion
    #region Public Members
    public HttpClient Client { get => httpClient; private set => httpClient = value; }
    public ILogger Log { get => log; private set => log = value; }
    public string BaseUri => Authentication?.Uri ?? string.Empty;
    public string WebsocketBaseUri => Authentication?.WebsocketUri ?? string.Empty;
    public IAuthentication? Authentication { get => authentication; set => authentication = value; }
    #endregion
    #region Constructor
    public CBProService(ILogger logger)
    {
      authentication = AuthenticationConfig.GetAuthentication(Marketplaces.COINBASE_PRO);

      httpClient = new HttpClient()
      {
        BaseAddress = new Uri(Authentication?.Uri ?? ""),  
      };

      log = logger;

      Log.LogInformation("{0}: '{1}' has been instantiated.", DateTime.UtcNow, nameof(CBProService));
    }
    #endregion
    #region Public Methods
    public async Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      using (var request = new HttpRequestMessage(httpMethod, BaseUri + requestUri))
      {
        if(content is not null)
        {
          var signAndTimestamp = (ValueTuple<string, string>)content;
          request.Headers.Add("Accept", "application/json");
          request.Headers.Add("CB-ACCESS-KEY", Authentication?.Key);
          request.Headers.Add("CB-ACCESS-SIGN", signAndTimestamp.Item1);
          request.Headers.Add("CB-ACCESS-TIMESTAMP", signAndTimestamp.Item2);
          request.Headers.Add("CB-ACCESS-PASSPHRASE", Authentication?.Pass);

          request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        }

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

      string method = httpMethod.ToString();
      string body = JsonConvert.SerializeObject(query);
      string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

      var message = timestamp + method + requestUri + body;

      using var hash = SHA256.Create();
      var byteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(Authentication?.Secret ?? ""));
      string hmac = Convert.ToHexString(byteArray).ToLower();

      var signature = Sign(message, hmac);

      return await SendAsync(httpMethod, requestUri, (signature, timestamp));
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
