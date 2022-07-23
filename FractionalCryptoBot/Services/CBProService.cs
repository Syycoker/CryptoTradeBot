using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
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
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("USER-AGENT", "CoinbaseProClient");

        if (content is not null)
        {
          var signAndTimestamp = (ValueTuple<string, long>)content;
          request.Headers.Add("CB-VERSION", "2015-04-08");
          request.Headers.Add("CB-ACCESS-KEY", Authentication?.Key);
          request.Headers.Add("CB-ACCESS-SIGN", signAndTimestamp.Item1);
          request.Headers.Add("CB-ACCESS-TIMESTAMP", signAndTimestamp.Item2.ToString("F0", CultureInfo.InvariantCulture));
          request.Headers.Add("CB-ACCESS-PASSPHRASE", Authentication?.Pass);
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
      DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
      long timestamp = (long)Math.Floor(diff.TotalSeconds);

      var computedSignature = ComputeSignature(httpMethod,
        Authentication?.Secret ?? "", timestamp, requestUri,
        JsonConvert.SerializeObject(query));

      //string method = httpMethod.ToString();
      //string body = JsonConvert.SerializeObject(query);
      //var prehash = timestamp.ToString("F0", CultureInfo.InvariantCulture) + httpMethod.ToString().ToUpper() + requestUri + body;
      //var sign = Sign(Authentication?.Secret ?? "" , prehash);

      return await SendAsync(httpMethod, requestUri, 
        (computedSignature, timestamp));
    }

    public void ParseWebsocketPayload(Crypto crypto, string content)
    {
      throw new NotImplementedException();
    }

    public string ComputeSignature(
    HttpMethod httpMethod,
    string secret,
    double timestamp,
    string requestUri,
    string contentBody = "")
    {
      var convertedString = Convert.FromBase64String(secret);
      var prehash = timestamp.ToString("F0", CultureInfo.InvariantCulture) + httpMethod.ToString().ToUpper() + requestUri + contentBody;
      return HashString(prehash, convertedString);
    }

    private string HashString(string str, byte[] secret)
    {
      var bytes = Encoding.UTF8.GetBytes(str);
      using (var hmaccsha = new HMACSHA256(secret))
      {
        return Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
      }
    }

    public string Sign(string secretKey, string data)
    {
      var secret = Convert.FromBase64String(secretKey);

      var bytes = Encoding.UTF8.GetBytes(data);
      using (var hmaccsha = new HMACSHA256(secret))
      {
        return System.Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
      }
    }

    public string GetWebsocketPath(params string[] content)
    {
      return $"{Authentication?.WebsocketUri}";
    }
    #endregion
  }
}
