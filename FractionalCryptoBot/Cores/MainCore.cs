using FractionalCryptoBot.Configuration;
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
    public async Task GetLowestPriceOfAsset(string symbol)
    {
      var cores = CoreFactory.GetCores();

      IEnumerator core = cores.GetEnumerator();

      Crypto cryptoToBeBought = null;

      decimal lowestPricedAsset = 0;

      ICore? exchangeAssetCameFrom = null;
        
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

          // Set
          cryptoToBeBought = assetDto;
          exchangeAssetCameFrom = c;
        }
      }

      if (cryptoToBeBought is null || exchangeAssetCameFrom is null)
        return;

      exchangeAssetCameFrom.BuyAsset(cryptoToBeBought, cryptoToBeBought.BiddingPrice);
    }
    #endregion
  }
}
