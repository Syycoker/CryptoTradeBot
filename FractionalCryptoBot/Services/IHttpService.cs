using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;

namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// An interface to enable communication between services such as "Binance, Crypto.com, Coinbase, etc.
  /// </summary>
  public interface IHttpService
  {
    #region Members
    /// <summary>
    /// The HttpClient for the service.
    /// </summary>
    public HttpClient Client { get; }

    /// <summary>
    /// The authentication for the service to make valid signed calls.
    /// </summary>
    public IAuthentication? Authentication { get; }

    /// <summary>
    /// The Logger for the service.
    /// </summary>
    public ILogger Log { get; }

    /// <summary>
    /// The base uri for the service.
    /// </summary>
    public string BaseUri { get; }

    /// <summary>
    /// The base uri for the service's websocket.
    /// </summary>
    public string WebsocketBaseUri { get; }

    /// <summary>
    /// The interval the stream from the marketplace will be checked at.
    /// </summary>
    public string KlineStreamInterval { get; }
    #endregion
    #region Public
    /// <summary>
    /// Sends a request to an endpoint and returns a string.
    /// </summary>
    /// <param name="httpMethod">What method the request is, i.e. 'GET', 'POST', 'DELETE'.</param>
    /// <param name="requestUri">The URL of the endpoint (where to send the request to).</param>
    /// <param name="content">The content to be sent alongside the request.</param>
    /// <returns>The response as a string.</returns>
    Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null);

    /// <summary>
    /// Sends a request to a service's endpoint without creating authorisation for the request.
    /// </summary>
    /// <param name="httpMethod">The method the reuqest adhere's to.</param>
    /// <param name="requestUri">The location of the specific endpoint.</param>
    /// <param name="query">The queries to the request.</param>
    /// <param name="content">The content to be sent alongside the request.</param>
    /// <returns>The response as a string.</returns>
    Task<string> SendPublicAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null);

    /// <summary>
    /// Sends a signed request to the service's endpoint with authorisation.
    /// </summary>
    /// <param name="httpMethod">The method the reuqest adhere's to.</param>
    /// <param name="requestUri">The location of the specific endpoint.</param>
    /// <param name="query">The queries to the request.</param>
    /// <param name="content">The content to be sent alongside the request.</param>
    /// <returns>The response as a string.</returns>
    Task<string> SendSignedAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null);

    /// <summary>
    /// Returns a new Websocket to be used.
    /// </summary>
    /// <returns></returns>
    ClientWebSocket CreateWebSocket()
    {
      return new ClientWebSocket();
    }

    /// <summary>
    /// Returns the websocket endpoint and the parameters of an exchange.
    /// </summary>
    /// <returns></returns>
    ValueTuple<string, string> GetWebsocketEndpoint();

    /// <summary>
    /// Parses the websocket payload for a specific marketplace.
    /// </summary>
    /// <param name="payload"></param>
    void ParseWebsocketPayload(Crypto crypto, string payload);

    /// <summary>
    /// HMAC signs the string if provided a source and its secret.
    /// </summary>
    /// <param name="source">The source string to then be encrypted.</param>
    /// <param name="key">The secret key to encrypt the source.</param>
    /// <returns>An encrypted string.</returns>
    string Sign(string source, string key);
    #endregion
  }
}
