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
    public decimal BaseBiddingPrice { get; private set; } = 0.00m;

    /// <summary>
    /// The current bidding price for the quote asset.
    /// </summary>
    public decimal QuoteBiddingPrice { get; private set; } = 0.00m;

    /// <summary>
    /// The current minimum buy price for the base asset.
    /// </summary>
    public decimal BaseMinimumBuyPrice { get; private set; } = 0.00m;

    /// <summary>
    /// The current minimum buy price for the quote asset.
    /// </summary>
    public decimal QuoteMinimumBuyPrice { get; private set; } = 0.00m;
    #endregion
    #region Constructors
    /// <summary>
    /// Default constructor for a crypto object regardless of marketplace.
    /// </summary>
    /// <param name="core"></param>
    /// <param name="baseName"></param>
    /// <param name="quoteName"></param>
    /// <param name="basePrecision"></param>
    /// <param name="quotePrecision"></param>
    /// <param name="baseBiddingPrice"></param>
    /// <param name="quoteBiddingPrice"></param>
    /// <param name="baseMinimumBuyPrice"></param>
    /// <param name="quoteMinimumBuyPrice"></param>
    public Crypto(ICore core, string baseName, string quoteName,
                  int basePrecision, int quotePrecision,
                  decimal baseBiddingPrice, decimal quoteBiddingPrice,
                  decimal baseMinimumBuyPrice, decimal quoteMinimumBuyPrice)
    {
      // Setting corresponding fields & properties.

      Core = core; // The core / service the crypto belongs to.

      BaseName = baseName; // The names
      QuoteName = quoteName;

      BasePrecision = basePrecision; // The precisions
      QuotePrecision = quotePrecision;

      BaseBiddingPrice = baseBiddingPrice; // The bidding prices
      QuoteBiddingPrice = quoteBiddingPrice;

      BaseMinimumBuyPrice = baseMinimumBuyPrice; // The minimum buy prices
      QuoteMinimumBuyPrice = quoteMinimumBuyPrice;
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
    #endregion
  }
}
