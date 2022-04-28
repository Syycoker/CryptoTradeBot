using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Services;

namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// A DTO for cryptocurrencies.
  /// </summary>
  public class Crypto
  {
    /// <summary>
    /// The core which was used to instantiate this object.
    /// </summary>
    public readonly ICore Core;

    /// <summary>
    /// The name of the crptocurrency as its short version of its name, i.e. 'BTC' -> 'Bitcoin'.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The precision of the asset if/when placing a bid for it on a marketplace.
    /// </summary>
    public readonly int Precision = 0;

    /// <summary>
    /// The current bidding price of the crypto currency in the exchange its from.
    /// </summary>
    public readonly decimal BiddingPrice = 0;

    /// <summary>
    /// The minimum amount the crypto can be bought using the exchange.
    /// </summary>
    public readonly decimal MinimumBuyPrice = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="core">The core which references the crypto.</param>
    /// <param name="name">The name of the cryptocurrency as a 3-5 letter shorthand.</param>
    /// <param name="precision">The precision of the asset.</param>
    /// <param name="biddingPrice">The bidding price of the coin</param>
    /// <param name="minimumBuyPrice">The minimum buy price of the coin.</param>
    public Crypto(ICore core, string name, int precision, decimal biddingPrice, decimal minimumBuyPrice)
    {
      Core = core;
      Name = name;
      Precision = precision;
      BiddingPrice = biddingPrice;
      MinimumBuyPrice = minimumBuyPrice;
    }
  }
}
