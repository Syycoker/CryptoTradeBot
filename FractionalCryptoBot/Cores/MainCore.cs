using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public void RunMainProcedure()
    {
      try
      {

      }
      catch
      {
        // Reboot procedure if you get certain exceptions.
        Logger.LogError("{0}: An error has occured in '{1}', attempting to reboot...", DateTime.UtcNow, nameof(RunMainProcedure));
      }
    }

    /// <summary>
    /// Gets all the names of cryptocurrenies that may be available for sale on all the marketplaces.
    /// </summary>
    /// <returns></returns>
    public async Task<List<IEnumerable<Crypto>>> GetAllCryptoCurrencies()
    {
      List<IEnumerable<Crypto>> crypto = new();
      try
      {
        // Get all the cores available.
        IEnumerable<ICore> cores = CoreFactory.GetCores();

        // Get a collection of all the cryptocurrencies in each core.
        foreach (var core in cores)
        {
          IEnumerable<Crypto> currencies = await core.GetCryptoCurrencies();
          crypto.Add(currencies);
        }

        // Return the collection.
        return crypto;
      }
      catch(Exception e)
      {
        // Swallow the exception and return which core threw this error.
        Logger.LogError("{0}: Could not get cryptocurrencies from '{1}.", DateTime.UtcNow, e.Message);
        return crypto;
      }
    }

    /// <summary>
    /// Returns all commmon crypto currencies using a greedy approach.
    /// </summary>
    /// <returns>A collection of SharedCrypto DTOs.</returns>
    public IEnumerable<SharedCrypto> GetCommonCryptocurrencies(IEnumerable<string> cryptoCurrencies)
    {
      // Go through each crypto in the collection and feed each one of them to the cores's services to see
      // If the core's marketplace does indeed have that particualr crypto.
      // If it does, then create a DTO for each of the common cryptos on multiple marketplaces.

      // Get all the cores available to make the calls...
      IEnumerable<ICore> cores = CoreFactory.GetCores();

      // Use LINQ to return the common cryptos...
      IEnumerable<SharedCrypto> sharedCrypto = cryptoCurrencies
        .Select(crypto => new SharedCrypto(
          cores.Select(core =>
            core.GetCryptoCurrency(crypto).Result
            )));

      return sharedCrypto;
    }

    /// <summary>
    /// Returns the cryptocurrency which is the lowest denomination from multiple other of it's type in different marketplaces.
    /// </summary>
    /// <param name="sharedCryptos"></param>
    /// <returns></returns>
    public Crypto? GetLowestPricedAsset(SharedCrypto sharedCryptos)
    {
      return sharedCryptos.GetLowestBiddingAsset();
    }

    /// <summary>
    /// Initiates a market order for the cryptocurrenc in their respective marketplace.
    /// </summary>
    /// <param name="crypto">The cryptocurrency to be bought.</param>
    /// <param name="price">The price to buy the cryptocurrency.</param>
    /// <param name="quantity">The optional variable to override the price.</param>
    /// <returns>A 'CoreStatus' enum to return how the process went.</returns>
    public async Task<CoreStatus> BuyAsset(Crypto crypto, decimal price, decimal quantity = 0.00m)
    {
      Logger.LogInformation("{0}: Attempting to buy asset '{1}' on the '{2}' marketplace.",
          DateTime.UtcNow,
          crypto.Name,
          nameof(crypto.Core.Service));

      return await crypto.Core.BuyAsset(crypto, price);
    }

    /// <summary>
    /// Initiates a market order for the cryptocurrenc in their respective marketplace.
    /// </summary>
    /// <param name="crypto">The cryptocurrency to be bought.</param>
    /// <param name="price">The price to buy the cryptocurrency.</param>
    /// <param name="quantity">The optional variable to override the price.</param>
    /// <returns>A 'CoreStatus' enum to return how the process went.</returns>
    public async Task<CoreStatus> SellAsset(Crypto crypto, decimal price, decimal quantity = 0.00m)
    {
      Logger.LogInformation("{0}: Attempting to sell asset '{1}' on the '{2}' marketplace.",
          DateTime.UtcNow,
          crypto.Name,
          nameof(crypto.Core.Service));

      return await crypto.Core.SellAsset(crypto, price);
    }
    #endregion
  }
}
