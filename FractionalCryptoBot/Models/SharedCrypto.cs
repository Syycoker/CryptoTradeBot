namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// DTO class to represent currencies that are common in however many other services.
  /// </summary>
  public class SharedCrypto
  {
    #region Members
    /// <summary>
    /// Immutable collection of crypto.
    /// </summary>
    public readonly IEnumerable<Crypto?> Cryptos;
    #endregion
    #region Constructor
    /// <summary>
    /// Public constructor that takes in a variable number of Crypto DTOs.
    /// </summary>
    /// <param name="cryptos"></param>
    public SharedCrypto(params Crypto[] cryptos)
    {
      Cryptos = cryptos.Where(crypto => crypto is not null);
    }

    /// <summary>
    /// Public constructorthat takes in a collection of Crypto DTOs.
    /// </summary>
    /// <param name="cryptos"></param>
    public SharedCrypto(IEnumerable<Crypto?> cryptos)
    {
      Cryptos = cryptos.Where(crypto => crypto is not null);
    }
    #endregion
    #region Public
    /// <summary>
    /// Returns the asset with the lowest base price in the collection stored.
    /// </summary>
    /// <returns>The lowest bidding asset from multiple exchanges.</returns>
    public Crypto? GetLowestBaseBiddingAsset()
    {
      return Cryptos.MinBy(crypto => crypto?.BaseBiddingPrice);
    }

    /// <summary>
    /// Returns the asset with the lowest quote price in the collection stored.
    /// </summary>
    /// <returns></returns>
    public Crypto? GetLowestQuoteBiddingPriceAsset()
    {
      return Cryptos.MinBy(crypto => crypto?.QuoteBiddingPrice);
    }
    #endregion
  }
}
