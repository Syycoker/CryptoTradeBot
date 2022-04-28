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
        Logger.LogError("{0}: An error has occured in '{1}', attempting to reboot.", DateTime.UtcNow, nameof(RunMainProcedure));
      }
    }

    /// <summary>
    /// Gets all the names of cryptocurrenies that may be available for sale on all the marketplaces.
    /// </summary>
    /// <returns></returns>
    public async Task GetCryptoCurrencies()
    {
      try
      {
        // List<string> currencies = await;

        // currencies.ForEach(currency => Task.Run());
      }
      catch
      {
        // Swallow the exception
      }
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
        var lowestPricedAsset = await GetLowestPriceOfAsset(symbol);

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
        var assetDto = await c.GetDTOFromAsset(symbol);

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
