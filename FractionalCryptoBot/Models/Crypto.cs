using FractionalCryptoBot.Cores;

namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// A DTO for cryptocurrencies found on a marketplace.
  /// </summary>
  public class Crypto
  {
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
                  int basePrecision, int quotePrecision)
    {
      // Setting corresponding fields & properties.

      Core = core; // The core / service the crypto belongs to.

      BaseName = baseName; // The names
      QuoteName = quoteName;

      BasePrecision = basePrecision; // The precisions
      QuotePrecision = quotePrecision;

      // On construction of the object, bootupp the websocket in its core to update the bidding price &
      // minimum buy price in the background.
    }
    #endregion
    #region Public Methods
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
  }
}
