using FractionalCryptoBot.Enumerations;
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
    private const int PROMISING_TRANSACTION_POLL_TIME = 3500;

    /// <summary>
    /// If the collection may not make us money, how frequent should I check the collection to repeat the procedure?
    /// </summary>
    private const int NON_PROMISING_TRANSACTION_POLL_INTERVAL = 35000;

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
    private const decimal MIN_PROFIT = 1.3m;
    private const decimal PROFIT_THRESHOLD = 0.1m;
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

      // Start the websocket for each crypto...
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

      // Start the websocket for each crypto...
    }
    #endregion
    #region Private
    /// <summary>
    /// Determines the performance of an asset based on its metrics.
    /// </summary>
    /// <param name="crypto"></param>
    /// <returns></returns>
    private AnalysisEval DeterminePerformance(Crypto crypto)
    {
      return AnalysisEval.NONE;
    }
    #endregion
    #region Public
    /// <summary>
    /// Runs the stream for each crypto.
    /// </summary>
    public void RunStreams() => Cryptos.Select(crypto => crypto.RunStream());

    /// <summary>
    /// Checks the status of each collection after the interval period has secceeded.
    /// </summary>
    /// <returns>N/A</returns>
    public async Task CheckStatus()
    {
      while (ShouldCheckStatus)
      {
        await Task.Delay(CHECK_STATUS_POLLING_INTERVAL);
        // Get the asset that's the bidding for the lowest amount.

        if (Cryptos.Count <= 1) continue;

        Crypto? lowestAsset = GetLowestBaseBiddingAsset();
        Crypto? highestAsset = GetHighestBaseBiddingAsset();

        // If whatever we recieve is invalid, throw an exception up a level to the procedure that is calling.
        if (lowestAsset is null || highestAsset is null) throw new Exception(string.Format("Unable to justify the assets in the collection - '{1}'.", nameof(CheckStatus)));

        // Calculate the total amount of fees we will incur
        var lowExchangeMakerFee = lowestAsset.BidPrice * lowestAsset.Core.MakerFee;
        var highExchnageTakerFee = highestAsset.AskPrice * highestAsset.Core.TakerFee;

        var lowExchange = lowestAsset.BidPrice - lowExchangeMakerFee;
        var highExchange = highestAsset.AskPrice - highExchnageTakerFee;

        var differenceInPriceInPercent = highExchange / lowExchange;

        if (differenceInPriceInPercent < MIN_PROFIT + (MIN_PROFIT * PROFIT_THRESHOLD)) continue;

        // Buy and sell at the exact same time for now.
        Task.WaitAll(
          Task.Run(async ()=> await lowestAsset.BuyAsset()),
          Task.Run(async ()=> await highestAsset.SellAsset())
          );
      }
    }

    /// <summary>
    /// Sets the 'ShouldCheckStatus' to whatever the value is (can stop the 'CheckStatus' method from running).
    /// </summary>
    /// <param name="run"></param>
    public void Stop(bool run = false) => ShouldCheckStatus = run;

    /// <summary>
    /// Returns the asset with the lowest base bidding price in the collection stored.
    /// </summary>
    /// <returns>The lowest bidding asset from multiple exchanges.</returns>
    public Crypto? GetLowestBaseBiddingAsset() => Cryptos.MinBy(crypto => crypto.BidPrice);

    /// <summary>
    /// Returns the asset with the highest base bidding price in the collection stored.
    /// </summary>
    /// <returns></returns>
    public Crypto? GetHighestBaseBiddingAsset() => Cryptos.MaxBy(crypto => crypto.BidPrice);
    #endregion
  }
}
