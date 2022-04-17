namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// An interface to enable communication between services such as "Binance, Crypto.com, Coinbase, etc.
  /// </summary>
  public interface IHttpService
  {
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
    /// Sends a request to the websocket endpoint location.
    /// </summary>
    /// <param name="streamName">The name of the stream.</param>
    /// <returns>A response string, i.e. JSON.</returns>
    Task SendWebsocketAsync(string streamName);

    /// <summary>
    /// To handle an operation once the socket has been opened.
    /// </summary>
    /// <param name="sender">The object raising the event.</param>
    /// <param name="args">The arguments of the event.</param>
    void SocketOnOpen(object sender, EventArgs args);

    /// <summary>
    /// To handle an operation once the socket has sent/recieved a message.
    /// </summary>
    /// <param name="sender">The object raising the event.</param>
    /// <param name="args">The arguments of the event.</param>
    void SocketOnMessage(object sender, EventArgs args);


    /// <summary>
    /// To handle an operation once the socket has been closed.
    /// </summary>
    /// <param name="sender">The object raising the event.</param>
    /// <param name="args">The arguments of the event.</param>
    void SocketOnClose(object sender, EventArgs args);

    /// <summary>
    /// HMAC signs the string if provided a source and its secret.
    /// </summary>
    /// <param name="source">The source string to then be encrypted.</param>
    /// <param name="key">The secret key to encrypt the source.</param>
    /// <returns>An encrypted string.</returns>
    string Sign(string source, string key);
  }
}
