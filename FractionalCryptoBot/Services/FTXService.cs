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
  public class FTXService : IHttpService
  {
    #region Private Members
    private HttpClient httpClient;
    private IAuthentication? authentication;
    private ILogger log;
    #endregion
    #region Public Members
    public HttpClient Client { get => httpClient; private set => httpClient = value; }
    public IAuthentication? Authentication { get => authentication; set => authentication = value; }
    public ILogger Log { get => log; private set => log = value; }
    public string BaseUri => Authentication?.Uri ?? string.Empty;
    public string WebsocketBaseUri => Authentication?.WebsocketUri ?? string.Empty;
    #endregion
    #region Constructor
    public FTXService(ILogger logger)
    {
      authentication = AuthenticationConfig.GetAuthentication(Marketplaces.FTX);

      httpClient = new HttpClient()
      {
        BaseAddress = new Uri(Authentication?.Uri ?? ""),
      };

      log = logger;

      Log.LogInformation("{0}: '{1}' has been instantiated.", DateTime.UtcNow, nameof(FTXService));
    }
    #endregion
    public string GetWebsocketPath(params string[] content)
    {
      throw new NotImplementedException();
    }

    public void ParseWebsocketPayload(Crypto crypto, string content)
    {
      throw new NotImplementedException();
    }

    public async Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      using (var request = new HttpRequestMessage(httpMethod, BaseUri + requestUri))
      {
        request.Headers.Add("Accept", "application/json");

        if (content is not null)
        {
          var signAndTimestamp = (ValueTuple<string, long>)content;
          request.Headers.Add("FTX-KEY", Authentication?.Key);
          request.Headers.Add("FTX-SIGN", signAndTimestamp.Item1);
          request.Headers.Add("FTX-TS", signAndTimestamp.Item2.ToString());
        }
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

      var signaturePayload = $"{now}{httpMethod.ToString().ToUpper()}{requestUri}";

      string signature = Sign(signaturePayload, Authentication?.Secret ?? "");

      StringBuilder requestUriBuilder = new StringBuilder(requestUri);
      requestUriBuilder.Append("?").Append(queryStringBuilder.ToString());

      return await SendAsync(httpMethod, requestUriBuilder.ToString(), content);
    }

    public string Sign(string source, string secret)
    {
      var hashMaker = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
      var hash = hashMaker.ComputeHash(Encoding.UTF8.GetBytes(source));
      var hashString = BitConverter.ToString(hash).Replace("-", string.Empty);
      return hashString.ToLower();
    }
  }
}
