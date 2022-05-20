using FractionalCryptoBot.Enumerations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// The main core's http service.
  /// </summary>
  public class MainCoreService : HttpService
  {
    public MainCoreService(ILogger logger, Marketplaces marketplace) : base(logger, marketplace)
    {
      Log.LogInformation("{0}: '{1}' has been instantiated.", DateTime.UtcNow, nameof(MainCoreService));
    }

    public override Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      throw new NotImplementedException();
    }

    public override Task<string> SendPublicAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
    {
      throw new NotImplementedException();
    }

    public override Task<string> SendSignedAsync(HttpMethod httpMethod, string requestUri, Dictionary<string, object>? query = null, object? content = null)
    {
      throw new NotImplementedException();
    }

    public override void SendWebsocketAsync(string parameter)
    {
      throw new NotImplementedException();
    }

    public override void SocketOnClose(object? sender, EventArgs args)
    {
      throw new NotImplementedException();
    }

    public override void SocketOnMessage(object? sender, EventArgs args)
    {
      throw new NotImplementedException();
    }

    public override void SocketOnOpen(object? sender, EventArgs args)
    {
      throw new NotImplementedException();
    }
  }
}
