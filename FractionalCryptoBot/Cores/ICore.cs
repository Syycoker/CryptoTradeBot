using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FractionalCryptoBot.Cores
{
  /// <summary>
  /// Interface to work on the business logic of a specific marketplace, I.e. Binance, FTX, etc...
  /// </summary>
  public interface ICore
  {
    #region Members
    /// <summary>
    /// The total number of cryptocurrencies in the marketplace.
    /// </summary>
    int NumberOfCryptoCurrencies { get; }

    /// <summary>
    /// The total price change of all crypto in the last 24 hours in a marketplace.
    /// </summary>
    decimal TotalPriceChange { get; }

    /// <summary>
    /// The average price change for all the cryptocurrencies in the marketplace.
    /// </summary>
    decimal MarketplaceAverage { get; }

    /// <summary>
    /// The average volume of all cryptocurrencies in the exchange.
    /// </summary>
    decimal AverageVolume { get; }

    /// <summary>
    /// The fees for transactions that are considered 'making' the market.
    /// </summary>
    decimal MakerFee { get; }

    /// <summary>
    /// The fees for transactions that are considered 'taking' from the market.
    /// </summary>
    decimal TakerFee { get; }

    /// <summary>
    /// The logger to be used.
    /// </summary>
    ILogger Log { get; }

    /// <summary>
    /// The service to be used to make ALL restful calls.
    /// </summary>
    IHttpService Service { get; }
    #endregion
    #region Public Methods
    /// <summary>
    /// Checks whether the core's service is active.
    /// </summary>
    /// <returns></returns>
    Task<bool> ActiveService();

    /// <summary>
    /// Gets a collection of cryptocurrencies using the service that will be used to make a call on its endpoints.
    /// </summary>
    /// <returns>An enumerable collection of DTOs.</returns>
    Task<IEnumerable<Crypto>> GetCryptoCurrencies();

    /// <summary>
    /// Gets a collection of cryptocurrencies that are performing well in the marketplace.
    /// </summary>
    /// <returns>An enumerable collection of DTOs.</returns>
    IEnumerable<Crypto> GetPerformantCrypto();

    /// <summary>
    /// Attempts to retrieve a cryptocurrency from an exchange as  JObject.
    /// </summary>
    /// <param name="crypto">The short name of the crypto.</param>
    /// <returns>A Json Object representing everything about the crypto in the particualr exchange.</returns>
    Task<JObject> GetAsset(string crypto);

    /// <summary>
    /// Gets the bidding price of the asset in the exchange.
    /// </summary>
    /// <param name="cryptoJsonObject"></param>
    /// <returns>A decimal representing the price of the asset.</returns>
    Task<decimal> GetBiddingPrice(Crypto cryptoDTO);

    /// <summary>
    /// Attempts to get an asset froma marketplace and convert the response string to a DTO if found in the exchange to be used from the calling class (MainCore).
    /// </summary>
    /// <param name="crypto">The qualified name for the cryptocurrency.</param>
    /// <returns>A 'Crypto' DTO.</returns>
    Task<Crypto?> GetCryptoCurrency(string crypto);

    /// <summary>
    /// Attempts to buy a specifc crypto (provided the DTO) and the amount wanting to buy.
    /// </summary>
    /// <param name="crypto">The crypto to be bought.</param>
    /// <param name="quantity">The amount of the crypto to be bought.</param>
    /// <param name="price">The price to override the quantity to be bought.</param>
    /// <returns>A 'CoreStatus' to provide the upper layer calling this method how the procedure went.</returns>
    Task<CoreStatus> BuyAsset(Crypto crypto, decimal price, decimal quantity = 0.00m);

    /// <summary>
    /// Attempts to sell a specifc crypto (provided the DTO) and the amount wanting to buy.
    /// </summary>
    /// <param name="crypto">The crypto to be sold.</param>
    /// <param name="price">The amount of the crypto to be sold.</param>
    /// <returns>A 'CoreStatus' to provide the upper layer calling this method how the procedure went.</returns>
    Task<CoreStatus> SellAsset(Crypto crypto, decimal price, decimal quantity = 0.00m);

    /// <summary>
    /// Attempts to transfer the crypto to a 'cold wallet' (if provided).
    /// </summary>
    /// <returns>A 'CoreStatus' to let the calling body know of the result of the procedure.</returns>
    CoreStatus TransferAssetToColdWallet(Crypto crypto, int walletId);

    /// <summary>
    /// Attempts to transfer the crypto to a 'hot wallet' / a wallet in the exchange.
    /// </summary>
    /// <param name="crypto">The asset to be transferred.</param>
    /// <param name="walletId">The identifier for a wallet in the exchange.</param>
    /// <returns>A 'CoreStatus' to let the calling body know of the result of the procedure.</returns>
    Task<CoreStatus> TransferAssetToExchange(Crypto crypto, string walletId);
    #endregion
  }
}
