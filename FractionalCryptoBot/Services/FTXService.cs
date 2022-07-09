using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
    public string GetWebsocketPath(params string[] content)
    {
      throw new NotImplementedException();
    }

    public void ParseWebsocketPayload(Crypto crypto, string content)
    {
      throw new NotImplementedException();
    }

    public Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      throw new NotImplementedException();
    }

    public Task<string> SendPublicAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
    {
      throw new NotImplementedException();
    }

    public Task<string> SendSignedAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
    {
      throw new NotImplementedException();
    }

    public string Sign(string source, string key)
    {
      var hashMaker = new HMACSHA256(Encoding.UTF8.GetBytes(Authentication?.Secret ?? string.Empty));
      var signaturePayload = $"{_nonce}{method.ToString().ToUpper()}{endpoint}";
      var hash = hashMaker.ComputeHash(Encoding.UTF8.GetBytes(signaturePayload));
      var hashString = BitConverter.ToString(hash).Replace("-", string.Empty);
      var signature = hashString.ToLower();
    }
  }
}
