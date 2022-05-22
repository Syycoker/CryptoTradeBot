using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// Static class to manage buying or selling an asset.
  /// </summary>
  public static class AssetManager
  {
    /// <summary>
    /// Allows the user to buy an asset using a merket order.
    /// </summary>
    /// <param name="crypto"></param>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static async Task BuyAsset(this Crypto crypto, decimal price, decimal quantity = 0.00m)
    {
      CoreStatus procedureResult = CoreStatus.NONE;

      if (quantity == 0.00m) procedureResult = await crypto.Core.BuyAsset(crypto, price);
      else procedureResult = await crypto.Core.BuyAsset(crypto, price, quantity);

      if (procedureResult != CoreStatus.BUY_SUCCESSFUL) HandleBuyOrderError(procedureResult);
    }

    /// <summary>
    /// What should happen if we recieved a core status that wasn't what we anticipated?
    /// </summary>
    /// <param name="errorStatus"></param>
    public static void HandleBuyOrderError(CoreStatus errorStatus)
    {
      switch (errorStatus)
      {

      }
    }
  }
}
