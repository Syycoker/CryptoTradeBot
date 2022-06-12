using FractionalCryptoBot.Cores;
using System.Net.WebSockets;

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
    private const ushort WEBSOCKET_BYTE_COUNT = 256;
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

    /// <summary>
    /// The current bidding price for the base asset.
    /// </summary>
    public decimal BaseBiddingPrice { get; private set; } = decimal.Zero;

    /// <summary>
    /// The current bidding price for the quote asset.
    /// </summary>
    public decimal QuoteBiddingPrice { get; private set; } = decimal.Zero;

    /// <summary>
    /// The current minimum buy price for the base asset.
    /// </summary>
    public decimal BaseMinimumBuyPrice { get; private set; } = decimal.Zero;

    /// <summary>
    /// The current minimum buy price for the quote asset.
    /// </summary>
    public decimal QuoteMinimumBuyPrice { get; private set; } = decimal.Zero;

    /// <summary>
    /// The marketcap for this particular crypto, i.e. how many are circulating * price of each individual asset.
    /// </summary>
    public decimal MarketCap { get; private set; } = decimal.Zero;

    /// <summary>
    /// What's the trend of the cryptocurrency in this particular exchange, i.e. negative volume change means less people are trading it than in the last 24 hours.
    /// </summary>
    public decimal VolumeChange { get; private set; } = decimal.Zero;

    /// <summary>
    /// Returns the pair name for this asset.
    /// </summary>
    /// <returns>A string representation of the asset in the exchange.</returns>
    public string PairName { get; private set; } = string.Empty;
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
                  int basePrecision, int quotePrecision, string pairName = "")
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
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Handles the message read from the websocket.
    /// </summary>
    /// <param name="buffer">The data being recieved.</param>
    /// <param name="count">The amount of bytes (int).</param>
    private void HandleMessage(byte[] buffer, int count)
    {
      string messageRecieved = BitConverter.ToString(buffer, 0, count);

      // Handle the payload by exchange implementation
      Core.Service.ParseWebsocketPayload(this, messageRecieved);
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Using the propertys from the model itself, 
    /// </summary>
    public async Task RunStream()
    {
      CancellationTokenSource source = new CancellationTokenSource();

      using (var ws = Core.Service.CreateWebSocket())
      {
        string pair = PairName.ToLower();
        var wssEndpoint = Core.Service.GetWebsocketEndpoint();
        string socketRequest = (wssEndpoint.Item1 + pair + wssEndpoint.Item2).ToLower();

        await ws.ConnectAsync(new Uri(socketRequest), CancellationToken.None);
        byte[] buffer = new byte[WEBSOCKET_BYTE_COUNT];
        while (ws.State == WebSocketState.Open)
        {
          var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
          if (result.MessageType == WebSocketMessageType.Close)
          {
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
          }
          else
          {
            HandleMessage(buffer, result.Count);
          }
        }
      }
    }

    /// <summary>
    /// Public method to set the base asset's bidding price.
    /// </summary>
    /// <param name="baseBiddingPrice">The bidding price for the base asset.</param>
    public void SetBaseBiddingPrice(decimal baseBiddingPrice) => BaseBiddingPrice = baseBiddingPrice;

    /// <summary>
    /// Public method to set the quote asset's bidding price.
    /// </summary>
    /// <param name="quoteBiddingPrice">The bidding price for the quote asset.</param>
    public void SetQuoteBiddingPrice(decimal quoteBiddingPrice) => QuoteBiddingPrice = quoteBiddingPrice;

    /// <summary>
    /// Public method to set the base asset's minimum buy price.
    /// </summary>
    /// <param name="baseMinimumPrice">The minimum buy price for the base asset.</param>
    public void SetBaseMinimumPrice(decimal baseMinimumPrice) => BaseMinimumBuyPrice = baseMinimumPrice;

    /// <summary>
    /// Public method to set the quote asset's minimum buy price.
    /// </summary>
    /// <param name="quoteMinimumPrice">The minimum buy price for the quote asset.</param>
    public void SetQuoteMinimumPrice(decimal quoteMinimumPrice) => QuoteMinimumBuyPrice = quoteMinimumPrice;

    /// <summary>
    /// Sets the marketcap for this particular cryptocurrency trade pair.
    /// </summary>
    /// <param name="marketCap">The current marketcap for this DTO.</param>
    public void SetMarketCap(decimal marketCap) => MarketCap = marketCap;

    /// <summary>
    /// Sets the volume chane for this particual cryptocurrency trade pair.
    /// </summary>
    /// <param name="volumeChange">The current (24h) volume change for  this DTO.</param>
    public void SetVolumeChange(decimal volumeChange) => VolumeChange = volumeChange;
    #endregion
    #region Comments for implementation details
    //// Testing stream 'kline'.
    //string pair = "btcusdt";
    //string interval = "1m";
    //string socketRequest = $"{WebsocketBaseUri}/ws/{ pair }@{ parameter }_{ interval }";

    //  using (var socket = new WebSocket(socketRequest))
    //  {
    //    socket.OnOpen += SocketOnOpen;
    //  }
    #endregion
  }
}
