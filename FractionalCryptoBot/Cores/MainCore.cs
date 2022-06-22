using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using Microsoft.Extensions.Logging;

namespace FractionalCryptoBot.Cores
{
  /// <summary>
  /// The class which will 'feed' symbols of cryptocurrencies into all cores available,
  /// handle all other running cores and handle the top level business logic for those cores.
  /// </summary>
  public class MainCore
  {
    #region Members
    /// <summary>
    /// The logger for the class
    /// </summary>
    ILogger Logger;
    #endregion
    #region Constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="logger">An implementation of a logger.</param>
    public MainCore(ILogger logger)
    {
      Logger = logger;
      Logger.LogInformation("{0}: '{1}' has been instantiated.", DateTime.UtcNow, nameof(MainCore));
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Runs the main procedure.
    /// </summary>
    public async Task RunMainProcedure()
    {
      try
      {
        // Get all the cryptocurrencies in all the cores in the system.
        var allExchangeCryptocurrencies = await GetAllCryptoCurrencies();

        // Check if we have any cryptocurrencies at all...
        if (allExchangeCryptocurrencies is null || allExchangeCryptocurrencies.Count() == 0) throw new Exception($"No cryptocurrencies found in any exchange - '{nameof(RunMainProcedure)}'.");

        // Group all the crypotcurrencies that can be found in more than one exchange/marketplace.
        var sharedCryptos = GetCommonCryptocurrencies(allExchangeCryptocurrencies);

        // Check if we have any shared cryptocurrencies...
        if (sharedCryptos is null || sharedCryptos.Count() == 0) throw new Exception($"No common cryptocurrencies found in any of the exchanges - '{nameof(RunMainProcedure)}'.");

        // Open the websockets...
        RunStreams(sharedCryptos);

        // Run each procedure...
        CheckPerformanceOfCryptosInExchange(sharedCryptos);
      }
      catch(Exception e)
      {
        // Log the exception
        Logger.LogCritical("{0}: '{1}'.", DateTime.UtcNow, e.Message);

        // Reboot procedure if you get certain exceptions.
        Logger.LogCritical("{0}: An error has occured in '{1}', attempting to reboot...", DateTime.UtcNow, nameof(RunMainProcedure));
      }
    }

    /// <summary>
    /// Gets all the names of cryptocurrenies that may be available for sale on all the marketplaces.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Crypto>> GetAllCryptoCurrencies()
    {
      try
      {
        // Get all the cores available.
        IEnumerable<ICore> cores = CoreFactory.GetCores();

        // Get a collection of all the cryptocurrencies in each core.
        IEnumerable<Crypto> collectedCrypto = (await Task.WhenAll(
                                                cores.Select(core => 
                                                core.GetCryptoCurrencies()))).SelectMany(currencies =>
                                                currencies).ToList();

        // Return the collection.
        return collectedCrypto;
      }
      catch(Exception e)
      {
        // Swallow the exception and return which core threw this error.
        Logger.LogError("{0}: Could not get cryptocurrencies from '{1}.", DateTime.UtcNow, e.Message);

        // Return an empty collection of crypto DTOs.
        return new List<Crypto>();
      }
    }

    /// <summary>
    /// Returns a collection of SharedCrypto DTOs where multiple exchanges have the sanem asset.
    /// </summary>
    /// <param name="cryptos"></param>
    /// <returns>A collection of  cryptocurrencies that exist on multiple exchanges.</returns>
    public IEnumerable<SharedCrypto> GetCommonCryptocurrencies(IEnumerable<Crypto> cryptos)
    {
      // Sort the collection by their base asset's name and their quote asset's name. (lexicographically)
      var sortedCurrencies = cryptos.OrderBy(crypto => crypto.BaseName).ThenBy(crypto => crypto.QuoteName);

      // Aggregate the identical assets.
      IEnumerable<SharedCrypto> sharedCrypto = sortedCurrencies
                                                .Select(crypto => 
                                                new SharedCrypto(
                                                  sortedCurrencies.Where(c => c.BaseName.Equals(crypto.BaseName)
                                                  && c.QuoteName.Equals(crypto.QuoteName))));

      return sharedCrypto;
    }

    /// <summary>
    /// Initiates opening the websocket for each shared asset.
    /// </summary>
    /// <param name="cryptos"></param>
    public void RunStreams(IEnumerable<SharedCrypto> cryptos) 
      => cryptos.ToList().ForEach(crypto => crypto.RunStreams());

    /// <summary>
    /// Checks the current state of the cryptocurrencies in their respective exchanges and their metrics, i.e. 'price change, volume, etc'...
    /// </summary>
    /// <param name="sharedCrypto"></param>
    /// <returns></returns>
    public void CheckPerformanceOfCryptosInExchange(IEnumerable<SharedCrypto> sharedCrypto)
    {
      // Make each object run their specific task.
      sharedCrypto.Select(sc => sc.CheckStatus());
    }
    #endregion
  }
}
