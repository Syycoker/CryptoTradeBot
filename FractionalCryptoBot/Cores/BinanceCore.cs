using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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
      string activeResponse = await Service.SendPublicAsync(HttpMethod.Get, "/sapi/v1/system/status");
      return JObject.Parse(activeResponse)?["status"]?.Value<int>().Equals(0) ?? false; // '0' is normal, '1' is system maintainance
    }

    public async Task<Crypto?> GetCryptoCurrency(string crypto)
    {
      var parameters = new Dictionary<string, object>()
      {
        {
          "symbol", crypto
        }
      };

      string cryptoResponse = await Service.SendPublicAsync(HttpMethod.Get, "/api/v3/exchangeInfo", parameters);

      JObject cryptoJson = JObject.Parse(cryptoResponse);
      var jsonArray = cryptoJson["symbols"];

      if (jsonArray is null) return null;
      var item = jsonArray[0];
      if (item is null) return null;

      string symbol = item["symbol"]?.Value<string>() ?? string.Empty;
      string baseAsset = item["baseAsset"]?.Value<string>() ?? string.Empty;
      string quoteAsset = item["quoteAsset"]?.Value<string>() ?? string.Empty;
      int baseAssetPrecision = item["baseAssetPrecision"]?.Value<int>() ?? 0;
      int quoteAssetPrecision = item["quoteAssetPrecision"]?.Value<int>() ?? 0;

      return new Crypto(this, baseAsset, quoteAsset, baseAssetPrecision, quoteAssetPrecision, symbol);
    }

    public async Task<IEnumerable<Crypto>> GetCryptoCurrencies()
    {
      string cryptoResponses = await Service.SendPublicAsync(HttpMethod.Get, "/api/v3/exchangeInfo");

      JObject cryptoJsonArray = JObject.Parse(cryptoResponses);
      var jsonArray = cryptoJsonArray?["symbols"]?.Value<JArray>();
      if (jsonArray is null) return new List<Crypto>();

      return jsonArray.Select(arr => 
          new Crypto(this, arr?["baseAsset"]?.Value<string>() ?? "",
          arr?["quoteAsset"]?.Value<string>() ?? "", arr?["baseAssetPrecision"]?.Value<int>() ?? 0,
          arr?["quoteAssetPrecision"]?.Value<int>() ?? 0, arr?["symbol"]?.Value<string>() ?? ""));
    }

    public async Task<CoreStatus> BuyAsset(Crypto crypto, decimal price, decimal quantity = 0.00M)
    {
      Dictionary<string, object> stopLossParameter = new()
      {
        { "symbol",crypto.PairName },
        { "side", "BUY" },
        { "type", "STOP_LOSS" },
        { "quantity", 0 },
        { "stopPrice",crypto.BaseMinimumBuyPrice },
      };

      var buyResponse = await Service.SendSignedAsync
        (HttpMethod.Post, "/api/v3/order",
        stopLossParameter);

      Dictionary<string, CoreStatus> buyActivities = new Dictionary<string, CoreStatus>()
      {
        {"Stop loss orders are not supported for this symbol", CoreStatus.BUY_UNSUCCESSFUL },
        {"Stop loss orders are not supported for this symbol", CoreStatus.BUY_UNSUCCESSFUL },
        {"Stop loss orders are not supported for this symbol", CoreStatus.BUY_UNSUCCESSFUL },
      };

      return buyActivities[buyResponse];
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
