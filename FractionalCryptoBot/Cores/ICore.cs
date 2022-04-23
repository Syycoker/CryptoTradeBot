using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;

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
    double MarketplaceAverage { get; }

    /// <summary>
    /// The logger to be used.
    /// </summary>
    ILogger Log { get; }

    /// <summary>
    /// The service to be used to make ALL restful calls.
    /// </summary>
    IHttpService Service { get; }

    /// <summary>
    /// A public property to get all available cryptocurrencies in the marketplace the service uses.
    /// </summary>
    IEnumerable<Crypto> Cryptocurrencies { get; }
    #endregion
    #region Public Methods
    /// <summary>
    /// Gets a collection of cryptocurrencies using the service that will be used to make a call on its endpoints.
    /// </summary>
    /// <returns>An enumerable collection of DTOs.</returns>
    IEnumerable<Crypto> GetCryptoCurrencies();

    /// <summary>
    /// Gets a collection of cryptocurrencies that are performing well in the marketplace.
    /// </summary>
    /// <returns>An enumerable collection of DTOs.</returns>
    IEnumerable<Crypto> GetPerformantCrypto();

    /// <summary>
    /// Attempts to buy a specifc crypto (provided the DTO) and the amount wanting to buy.
    /// </summary>
    /// <param name="crypto">The crypto to be bought.</param>
    /// <param name="amount">The amount of the crypto to be bought.</param>
    /// <returns>A 'CoreStatus' to provide the upper layer calling this method how the procedure went.</returns>
    CoreStatus BuyAsset(Crypto crypto, double amount);

    /// <summary>
    /// Attempts to sell a specifc crypto (provided the DTO) and the amount wanting to buy.
    /// </summary>
    /// <param name="crypto">The crypto to be sold.</param>
    /// <param name="amount">The amount of the crypto to be sold.</param>
    /// <returns>A 'CoreStatus' to provide the upper layer calling this method how the procedure went.</returns>
    CoreStatus SellAsset(Crypto crypto, double amount);

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
    CoreStatus TransferAssetToExchange(Crypto crypto, string walletId);
    #endregion
  }
}
