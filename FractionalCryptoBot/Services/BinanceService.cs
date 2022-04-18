using System.Text;
using System.Web;
using FractionalCryptoBot.Enumerations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharp;

namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// The service class for binance.
  /// </summary>
  public sealed class BinanceService : HttpService
  {
    /// <summary>
    /// Default constuctor to instantiate a binance service object.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClient"></param>
    public BinanceService(ILogger logger, HttpClient httpClient) : base(logger, httpClient, Marketplaces.BINANCE)
    {
      // Nothing needs to be set in the constructor for now.
      // Not using string interpolation as I lose more valuable information and processing time when doing so.
      Log.LogInformation("{0}: '{1}' has been instantiated.", DateTime.UtcNow, nameof(BinanceService));
    }

    /// <summary>
    /// Helper method to send an asynchoronus call to binance's endpoints.
    /// </summary>
    /// <param name="httpMethod">'GET', 'POST', 'DELETE'.</param>
    /// <param name="requestUri">The address of the endpoint.</param>
    /// <param name="content">The content to go with the call.</param>
    /// <returns>A response string in JSON.</returns>
    public override async Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      using (var request = new HttpRequestMessage(httpMethod, BaseUri + requestUri))
      {
        request.Headers.Add("X-MBX-APIKEY", ApiKey);

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
    public override async Task<string> SendPublicAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
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
    public override async Task<string> SendSignedAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
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

      string signature = Sign(queryStringBuilder.ToString(), ApiSecret);
      queryStringBuilder.Append("&signature=").Append(signature);

      StringBuilder requestUriBuilder = new StringBuilder(requestUri);
      requestUriBuilder.Append("?").Append(queryStringBuilder.ToString());

      return await SendAsync(httpMethod, requestUriBuilder.ToString(), content);
    }

    /// <summary>
    /// Sends a request to binance's websocket endpoint.
    /// </summary>
    /// <param name="parameter">The name of the stream</param>
    /// <returns>A response string.</returns>
    public override async Task<string> SendWebsocketAsync(string parameter)
    {
      // Testing stream 'kline'.
      string pair = "btcusdt";
      string interval = "1m";
      string socketRequest =$"{WebsocketBaseUri}/ws/{ pair }@{ parameter }_{ interval }";

      using (var socket = new WebSocket(socketRequest))
      {
        socket.OnOpen += SocketOnOpen;
      }

      return socketRequest;
    }

    /// <summary>
    /// Handles an operation when the websocket has been opened.
    /// </summary>
    /// <param name="sender">The object raising the event.</param>
    /// <param name="args">The arguments of the event.</param>
    public override void SocketOnOpen(object? sender, EventArgs args)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Handles an operation when the websocket has recieved/sent a message.
    /// </summary>
    /// <param name="sender">The object raising the event.</param>
    /// <param name="args">The arguments of the event.</param>
    public override void SocketOnMessage(object? sender, EventArgs args)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Handles an operation when the websocket has been closed.
    /// </summary>
    /// <param name="sender">The object raising the event.</param>
    /// <param name="args">The arguments of the event.</param>
    public override void SocketOnClose(object? sender, EventArgs args)
    {
      throw new NotImplementedException();
    }
  }
}
