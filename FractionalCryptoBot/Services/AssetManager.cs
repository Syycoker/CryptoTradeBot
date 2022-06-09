using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Exceptions;
using FractionalCryptoBot.Models;

namespace FractionalCryptoBot.Services
{
  /// <summary>
  /// Static class to manage buying or selling an asset.
  /// </summary>
  public static class AssetManager
  {
    #region Members
    /// <summary>
    /// To make sure not cross-thread operations cause any issues when buying an asset.
    /// </summary>
    private static Mutex BuyMutex = new Mutex();

    /// <summary>
    /// To make sure no corss-thread operations cause any issues when selling an asset.
    /// </summary>
    private static Mutex SellMutex = new Mutex();
    #endregion
    #region Public
    /// <summary>
    /// Allows the user to buy an asset using a merket order.
    /// </summary>
    /// <param name="crypto">The cryptocurrency to be bought.</param>
    /// <param name="price">The price to buy the cryptocurrency.</param>
    /// <param name="quantity">The amount of which to buy the cryptocurrency.</param>
    /// <returns></returns>
    public static async Task<CoreStatus> BuyAsset(this Crypto crypto, decimal price = 0.00m, decimal quantity = 0.00m)
    {
      BuyMutex.WaitOne();
      try
      {
        CoreStatus procedureResult = CoreStatus.NONE;

        // If the user chooses to override the price or quantity, choose accordingly
        if (price != 0.00m || quantity != 0.00m)
        {
          procedureResult = await crypto.Core.BuyAsset(crypto, price, quantity);
        }
        else
        {
          // If nothing has been specified / overriden the buy the smallest amount of which we can buy.
          procedureResult = await crypto.Core.BuyAsset(crypto, crypto.BaseMinimumBuyPrice);
        }

        // Return the response up a level.
        return procedureResult;
      }
      finally
      {
        BuyMutex.ReleaseMutex();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="crypto">The crypto which to sell.</param>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static async Task<CoreStatus> SellAsset(this Crypto crypto, decimal price = 0.00m, decimal quantity = 0.00m)
    {
      SellMutex.WaitOne();
      try
      {
        if (price != 0.00m || quantity != 0.00m)
          return await crypto.SellAsset(price, quantity);
        else
          return await crypto.SellAsset(crypto.BaseMinimumBuyPrice);
      }
      finally
      {
        SellMutex.ReleaseMutex();
      }
    }

    /// <summary>
    /// What should happen if we recieved a core status that wasn't what we anticipated?
    /// </summary>
    /// <param name="errorStatus"></param>
    public static void HandleBuyOrderError(CoreStatus errorStatus)
    {
      switch (errorStatus)
      {
        default:
        case CoreStatus.NONE:
        case CoreStatus.INSUFFICIENT_FUNDS:
        case CoreStatus.BUY_UNSUCCESSFUL:
        case CoreStatus.SELL_UNSUCCESSFUL:
        case CoreStatus.ASSET_DOES_NOT_EXIST: throw new Exception();
        case CoreStatus.UNKNOWN_ERROR: throw new InvalidAuthenticationException();
        case CoreStatus.OUT_OF_SYNC: throw new OutOfSyncException();
      }
    }
    #endregion
  }
}
