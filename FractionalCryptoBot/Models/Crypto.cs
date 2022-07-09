using FractionalCryptoBot.Cores;
using System.Net.WebSockets;
using System.Text;

namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// A DTO for cryptocurrencies found on a marketplace.
  /// </summary>
  public class Crypto
  {
    #region Constants
    /// <summary>
    /// The maximum amount of data that can be stored in a byte.
    /// </summary>
    private const ushort WEBSOCKET_BYTE_COUNT = 512;
    #endregion
    #region Members
    /// <summary>
    /// The core which was used to instantiate this object.
    /// </summary>
    public readonly ICore Core;

    /// <summary>
    /// The name of the base asset in it's short format, i.e. 'BTC'.
    /// </summary>
    public readonly string BaseName;

    /// <summary>
    /// The name of the quote asset in it's short format, i.e. 'BTC'.
    /// </summary>
    public readonly string QuoteName;

    /// <summary>
    /// The precision of the base asset.
    /// </summary>
    public readonly int BasePrecision;

    /// <summary>
    /// The precision of the quote asset.
    /// </summary>
    public readonly int QuotePrecision;

    public decimal QuoteMinimumQuantity { get; private set; } = decimal.Zero;

    /// <summary>
    /// Returns the pair name for this asset.
    /// </summary>
    /// <returns>A string representation of the asset in the exchange.</returns>
    public string PairName { get; private set; } = string.Empty;

    /// <summary>
    /// Determines whether the stream is active or not.
    /// </summary>
    public bool StreamActive { get; private set; } = false;

    public decimal BidPrice { get; private set; }
    public decimal BidQty { get; private set; }
    public decimal AskPrice { get; private set; }
    public decimal AskQty { get; private set; }

    private Thread? StreamThread { get; set; }
    #endregion
    #region Constructor
    /// <summary>
    /// Default constructor for a crypto object regardless of marketplace.
    /// </summary>
    /// <param name="core">The marketplace the coin belongs to.</param>
    /// <param name="baseName">e.g. 'BTC'.</param>
    /// <param name="quoteName">e.g. 'USD'.</param>
    /// <param name="basePrecision">e.g.'8' (0.00000001).</param>
    /// <param name="quotePrecision">e.g.'8' (0.00000001).</param>
    public Crypto(ICore core, string baseName, string quoteName,
                  int basePrecision, int quotePrecision, string pairName = "",
                  decimal quoteMinimumQuantity = 0.00m)
    {
      // Setting corresponding fields & properties.

      Core = core; // The core / service the crypto belongs to.

      BaseName = baseName; // The names
      QuoteName = quoteName;

      BasePrecision = basePrecision; // The precisions
      QuotePrecision = quotePrecision;

      PairName = string.IsNullOrEmpty(pairName) ? BaseName + QuoteName : pairName;
      // On construction of the object, bootup the websocket in its core to update the bidding price &
      // minimum buy price in the background.

      QuoteMinimumQuantity = quoteMinimumQuantity;
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Using the propertys from the model itself, 
    /// </summary>
    public async Task RunStream()
    {
      if (StreamActive || StreamThread is not null) return;
      StreamActive = true;

      Thread thread = new Thread(() => Stream())
      {
        IsBackground = true,
        Name = $"{PairName}-Stream",
      };
      thread.Start();
    }

    /// <summary>
    /// Stops the stream if it's currently running.
    /// </summary>
    public void StopStream() => StreamActive = false;

    public void SetBidPrice(decimal bidPrice) => BidPrice = bidPrice;
    public void SetBidQty(decimal bidQty) => BidQty = bidQty;
    public void SetAskPrice(decimal askPrice) => AskPrice = askPrice;
    public void SetAskQty(decimal askQty) => AskQty = askQty;

    #endregion
    #region Private Methods
    private async Task Stream()
    {
      string socketRequest = Core.Service.GetWebsocketPath(PairName.ToLower(), "bookTicker");

      using (var ws = Core.Service.CreateWebSocket())
      {
        await ws.ConnectAsync(new Uri(socketRequest), CancellationToken.None);

        byte[] buffer = new byte[WEBSOCKET_BYTE_COUNT];

        while (ws.State == WebSocketState.Open && StreamActive)
        {
          var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

          if (result.MessageType == WebSocketMessageType.Close) await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
          else
          {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Core.Service.ParseWebsocketPayload(this, message);
          }
        }
      }
    }
    #endregion
  }
}
