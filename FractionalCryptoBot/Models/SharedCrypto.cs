namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// DTO class to represent currencies that are common in however many other services.
  /// </summary>
  public class SharedCrypto
  {
    #region Constants
    /// <summary>
    /// The frequency (ms) that the shared crypto should be observed in their respective marketplaces.
    /// </summary>
    private const int CHECK_STATUS_POLLING_INTERVAL = 10000;
    #endregion
    #region Members
    /// <summary>
    /// Immutable collection of crypto.
    /// </summary>
    public readonly List<Crypto> Cryptos = new();

    /// <summary>
    /// Should the 'CheckStatus' operation continue?
    /// </summary>
    public bool ShouldCheckStatus { get; private set; } = true;
    #endregion
    #region Constructor
    /// <summary>
    /// Public constructor that takes in a variable number of Crypto DTOs.
    /// </summary>
    /// <param name="cryptos"></param>
    public SharedCrypto(params Crypto[] cryptos)
    {
      var nonNullCryptoObjs = cryptos.Where(crypto => crypto is not null);
      if (nonNullCryptoObjs is null || nonNullCryptoObjs.Count() == 0) return;

      Cryptos = nonNullCryptoObjs.ToList();
    }

    /// <summary>
    /// Public constructorthat takes in a collection of Crypto DTOs.
    /// </summary>
    /// <param name="cryptos"></param>
    public SharedCrypto(IEnumerable<Crypto> cryptos)
    {
      var nonNullCryptoObjs = cryptos.Where(crypto => crypto is not null);
      if (nonNullCryptoObjs is null || nonNullCryptoObjs.Count() == 0) return;

      Cryptos = nonNullCryptoObjs.ToList();
    }
    #endregion
    #region Public
    /// <summary>
    /// Checks the status of each collection after the interval period has secceeded.
    /// </summary>
    /// <returns>N/A</returns>
    public async Task CheckStatus()
    {
      while (ShouldCheckStatus)
      {
        await Task.Delay(CHECK_STATUS_POLLING_INTERVAL);

        // Get the average of the bidding prices between the collection
        var averageBiddingPrice = Cryptos.Average(crypto => crypto.BaseBiddingPrice);

        // Get the average volume from the collection
      }
    }

    /// <summary>
    /// Sets the 'ShouldCheckStatus' to whatever the value is (can stop the 'CheckStatus' method from running).
    /// </summary>
    /// <param name="run"></param>
    public void Run(bool run) => ShouldCheckStatus = run;

    /// <summary>
    /// Returns the asset with the lowest base price in the collection stored.
    /// </summary>
    /// <returns>The lowest bidding asset from multiple exchanges.</returns>
    public Crypto? GetLowestBaseBiddingAsset() => Cryptos.MinBy(crypto => crypto.BaseBiddingPrice);

    /// <summary>
    /// Returns the asset with the lowest quote price in the collection stored.
    /// </summary>
    /// <returns></returns>
    public Crypto? GetLowestQuoteBiddingPriceAsset() => Cryptos.MinBy(crypto => crypto.QuoteBiddingPrice);
    #endregion
  }
}
