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
          (cores.Select(core =>
            core.GetCryptoCurrency(crypto).Result
            ).Where(c => (c is not null) || !string.IsNullOrEmpty(c?.Name)))));

      return sharedCrypto;
    }

    /// <summary>
    /// Buys the lowest priced version of an asset from all available marketplaces.
    /// </summary>
    /// <param name="symbol">The symbol to be bought.</param>
    /// <returns>A 'CoreStatus' to relay what happened during the operation.</returns>
    public async Task<CoreStatus> BuyLowestPricedAsset(string symbol, decimal price)
    {
      try
      {
        // Try and get the lowest price of this symbol.
        var lowestPricedAsset = await GetLowestPriceOfAsset(symbol);

        // If we couldn't get one, return the corresponding enum.
        if (lowestPricedAsset is null) return CoreStatus.ASSET_DOES_NOT_EXIST;

        Logger.LogInformation("{0}: Initiating buy order for '{1}' in the '{2}'.", DateTime.UtcNow,
          lowestPricedAsset.Name,
          nameof(lowestPricedAsset.Core.Service));
        
        // Initiate a buy order.
        return lowestPricedAsset.Core.BuyAsset(lowestPricedAsset, price);
      }
      catch
      {
        Logger.LogError("{0}: '{1}' has been bought", DateTime.UtcNow, nameof(MainCore));
        return CoreStatus.BUY_UNSUCCESSFUL;
      }
    }

    /// <summary>
    /// Attempts to return the lowest priced asset from all available marketplaces.
    /// </summary>
    /// <param name="symbol">The qualified name of the asset.</param>
    /// <returns>A Crypto DTO.</returns>
    /// <exception cref="ArgumentNullException">If the crypto could not be found in any of the marketplaces.</exception>
    public async Task<Crypto> GetLowestPriceOfAsset(string symbol)
    {
      var cores = CoreFactory.GetCores();

      IEnumerator core = cores.GetEnumerator();

      Crypto? cryptoToBeBought = null;

      decimal lowestPricedAsset = 0;

      while (core.MoveNext())
      {
        ICore c = (ICore)core;

        // Get a DTO if the asset exists in the exchange.
        var assetDto = await c.GetCryptoCurrency(symbol);

        if (assetDto is null)
          continue;

        if (assetDto.BiddingPrice.CompareTo(lowestPricedAsset) < 0)
        {
          lowestPricedAsset = assetDto.BiddingPrice;
          cryptoToBeBought = assetDto;
        }
      }

      if (cryptoToBeBought is null)
        throw new ArgumentNullException(nameof(cryptoToBeBought) + " is null.");

      return cryptoToBeBought;
    }
    #endregion
  }
}
