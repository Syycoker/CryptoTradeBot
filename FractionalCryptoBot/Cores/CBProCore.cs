using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FractionalCryptoBot.Cores
{
  /// <summary>
  /// Class to make CBPro api calls.
  /// </summary>
  public sealed class CBPro : ICore
  {
    #region Private Members
    private int numberOfCryptoCurrencies = 0;
    private decimal totalPriceChange = 0;
    private decimal marketplaceAverage = 0;
    private decimal averageVolume = 0.00m;
    private decimal makerFee = 0.00m;
    private decimal takerFee = 0.00m;
    #endregion
    #region Public Members
    /// <summary>
    /// The logger for the core.
    /// </summary>
    public ILogger Log { get; init; }

    /// <summary>
    /// The service the core uses.
    /// </summary>
    public IHttpService Service { get; set; }

    /// <summary>
    /// The number of cryptocurrencies in the Coinbase Pro marketplace.
    /// </summary>
    public int NumberOfCryptoCurrencies { get => numberOfCryptoCurrencies; private set => numberOfCryptoCurrencies = value; }
            //https://api.exchange.coinbase.com/currencies

    /// <summary>
    /// The price change of all cryptocurrency in the Coinbase Pro marketplace.
    /// </summary>
    public decimal TotalPriceChange { get => totalPriceChange; private set => totalPriceChange = value; }

    /// <summary>
    /// The average amount any cryptocurrency in the Coinbase Pro marketplace is bidding for.
    /// </summary>
    public decimal MarketplaceAverage { get => marketplaceAverage; private set => marketplaceAverage = value; }

    /// <summary>
    /// The average volume of cryptocurrency in the Coinbase Pro marketplace.
    /// </summary>
    public decimal AverageVolume { get => averageVolume; private set => averageVolume = value; }

    /// <summary>
    /// The maker fee for the account.
    /// </summary>
    public decimal MakerFee { get => makerFee; private set => makerFee = value; }

    /// <summary>
    /// The taker fee for the account.
    /// </summary>
    public decimal TakerFee { get => takerFee; private set => takerFee = value; }
    #endregion
    #region Constructor
    /// <summary>
    /// Default constructor for Coinbase Pro.
    /// </summary>
    /// <param name="logger"></param>
    public CBProCore(ILogger logger)
    {
      // Setting the logger and the service to make http calls to the respectve service's endpoints.
      Log = logger;
      Service = new CBProService(logger);
    }
    #endregion
    #region Public Methods
    public async Task<bool> ActiveService()
    {
      string activeResponse = await Service.SendPublicAsync(HttpMethod.Get, "/sapi/v1/system/status");
      return JObject.Parse(activeResponse)["status"].Value<int>().Equals(0); // '0' is normal, '1' is system maintainance
    }

    public Task<Crypto?> GetCryptoCurrency(string crypto)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Crypto>> GetCryptoCurrencies()
    {
      throw new NotImplementedException();
    }

    public Task<CoreStatus> BuyAsset(Crypto crypto, decimal price, decimal quantity = 0.00M)
    {
      throw new NotImplementedException();
    }

    public Task<CoreStatus> SellAsset(Crypto crypto, decimal price, decimal quantity = 0.00M)
    {
      throw new NotImplementedException();
    }

    public CoreStatus TransferAssetToColdWallet(Crypto crypto, int walletId)
    {
      throw new NotImplementedException();
    }

    public Task<CoreStatus> TransferAssetToExchange(Crypto crypto, string walletId)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
