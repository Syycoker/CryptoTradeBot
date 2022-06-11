using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace FractionalCryptoBot.Cores
{
  /// <summary>
  /// Class to make binance api calls.
  /// </summary>
  public sealed class BinanceCore : ICore
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
    /// The number of cryptocurrencies in the binance marketplace.
    /// </summary>
    public int NumberOfCryptoCurrencies { get => numberOfCryptoCurrencies; private set => numberOfCryptoCurrencies = value; }

    /// <summary>
    /// The price change of all cryptocurrency in the binance marketplace.
    /// </summary>
    public decimal TotalPriceChange { get => totalPriceChange; private set => totalPriceChange = value; }

    /// <summary>
    /// The average amount any cryptocurrency in the binance marketplace is bidding for.
    /// </summary>
    public decimal MarketplaceAverage { get => marketplaceAverage; private set => marketplaceAverage = value; }

    /// <summary>
    /// The average volume of cryptocurrency in the binance marketplace.
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
    /// Default constructor for Binance core.
    /// </summary>
    /// <param name="logger"></param>
    public BinanceCore(ILogger logger)
    {
      // Setting the logger and the service to make http calls to the respectve service's endpoints.
      Log = logger;
      Service = new BinanceService(logger);
    }
    #endregion
    #region Public Methods
    public async Task<bool> ActiveService()
    {
      using (var request = new HttpRequestMessage(HttpMethod.Get, Service.BaseUri + "/markets/BTCUSD"))
      {
        request.Headers.Add("X-MBX-APIKEY", Service.Authentication?.ApiKey);

        object content = request.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

        if (!(request.Content is null))
          request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await Service.Client.SendAsync(request);

        return response.IsSuccessStatusCode;
      }
    }

    public Task<Crypto?> GetCryptoCurrency(string crypto)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Crypto>> GetCryptoCurrencies()
    {
      Crypto crypto = GetCryptoCurrency("BTC-GBP");
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
