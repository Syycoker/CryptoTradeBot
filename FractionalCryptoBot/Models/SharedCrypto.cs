using FractionalCryptoBot.Services;

namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// DTO class to represent currencies that are common in however many other services.
  /// </summary>
  public class SharedCrypto
  {
    #region Constants
    /// <summary>
    /// If the collection may make us money, how frequent should I check the collections to repeat the procedure?
    /// </summary>
    private const int PROMISING_TRANSACTION_POLL_TIME = 10000;

    /// <summary>
    /// If the collection may not make us money, how frequent should I check the collection to repeat the procedure?
    /// </summary>
    private const int NON_PROMISING_TRANSACTION_POLL_INTERVAL = 100000;

    /// <summary>
    /// The lower end of a marketcap for a cryptocurrency to be classified as a risky investment;
    /// </summary>
    private const long SMALL_CAP_LOW = 300000000;

    /// <summary>
    /// The upper end of a marketcap of a cryptocurrency to be classiefied as a riskier investment;
    /// </summary>
    private const long SMALL_CAP_HIGH = SMALL_CAP_LOW * 10;

    /// <summary>
    /// The lower end of a marketcap for a cryptocurrency to be classified as a safe investment, but prone to risky.
    /// </summary>
    private const long MID_CAP_LOW = SMALL_CAP_HIGH + 1;

    /// <summary>
    /// The upper end of a marketcap for a cryptocurrency to be classified as a safe investment, but prone to risky.
    /// </summary>
    private const long MID_CAP_HIGH = MID_CAP_LOW * 3;
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

    /// <summary>
    /// The frequency (ms) that the shared crypto should be observed in their respective marketplaces.
    /// </summary>
    private int CHECK_STATUS_POLLING_INTERVAL = NON_PROMISING_TRANSACTION_POLL_INTERVAL;
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

        // (For the purpose of making sense, I will only be analysing the base asset...)

        // Get the asset that's the bidding for the lowest amount.
        Crypto? lowestAsset = GetLowestBaseBiddingAsset();
        Crypto? highestAsset = GetHighestBaseBiddingAsset();

        // If whatever we recieve is invalid, throw an exception up a level to the procedure that is calling.
        if (lowestAsset is null || highestAsset is null) throw new Exception(string.Format("Unable to justify the assets in the collection - '{1}'.", nameof(CheckStatus)));

        // Check if the volume change in the past 24h is positive...
        bool positiveVolumeChange = lowestAsset.VolumeChange.CompareTo(decimal.Zero) >= 0 ? true : false;

        // Get the fees of the cheaper of the two cryptocurrency.
        decimal transactionFee = lowestAsset.Core.TakerFee;

        // Calculate the difference between the lowest & highest asset (minus the fee) to see if we can make a profit.
        decimal difference = (highestAsset.BaseBiddingPrice - lowestAsset.BaseBiddingPrice) - transactionFee;

        bool continueBuyProcedure = difference.CompareTo(decimal.Zero) > 0;

        // If the difference is negative, we will lose money if we fulfil the buy order so, let's just sleep for a while...
        if (!continueBuyProcedure || !positiveVolumeChange)
        {
          // Check the collection and how it's doing less frequently.
          CHECK_STATUS_POLLING_INTERVAL = NON_PROMISING_TRANSACTION_POLL_INTERVAL;
          continue;
        }
        else
        {
          // Check the collection and how it's performing more frequently.
          CHECK_STATUS_POLLING_INTERVAL = PROMISING_TRANSACTION_POLL_TIME;
        }

        // Attempt to buy the asset.
        var buyResponse = await lowestAsset.BuyAsset();
      }
    }

    /// <summary>
    /// Sets the 'ShouldCheckStatus' to whatever the value is (can stop the 'CheckStatus' method from running).
    /// </summary>
    /// <param name="run"></param>
    public void Run(bool run) => ShouldCheckStatus = run;

    /// <summary>
    /// Returns the asset with the lowest base bidding price in the collection stored.
    /// </summary>
    /// <returns>The lowest bidding asset from multiple exchanges.</returns>
    public Crypto? GetLowestBaseBiddingAsset() => Cryptos.MinBy(crypto => crypto.BaseBiddingPrice);

    /// <summary>
    /// Returns the asset with the highest base bidding price in the collection stored.
    /// </summary>
    /// <returns></returns>
    public Crypto? GetHighestBaseBiddingAsset() => Cryptos.MaxBy(crypto => crypto.BaseBiddingPrice);

    /// <summary>
    /// Returns the asset with the lowest quote price in the collection stored.
    /// </summary>
    /// <returns></returns>
    public Crypto? GetLowestQuoteBiddingPriceAsset() => Cryptos.MinBy(crypto => crypto.QuoteBiddingPrice);

    /// <summary>
    /// Returns the asset with the highest quote bidding price in the collection stored.
    /// </summary>
    /// <returns></returns>
    public Crypto? GetHighestQuoteBiddingAsset() => Cryptos.MaxBy(crypto => crypto.QuoteBiddingPrice);
    #endregion
  }
}
