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
        { "symbol", crypto }
      };

      string cryptoResponse = await Service.SendPublicAsync(HttpMethod.Get, "/api/v3/exchangeInfo", parameters);

      JObject cryptoJson = JObject.Parse(cryptoResponse);
      var item = cryptoJson?["symbols"]?.Value<JArray>()?[0];

      var lotSize = item?["filters"]?.Value<JArray>()?
        .Single(filter => (filter?["filterType"]?.Value<string>() ?? "")
        .Equals("LOT_SIZE"));

      var minQty = lotSize?["minQty"]?.Value<decimal>() ?? 0.00m;
      var stepSize = lotSize?["stepSize"]?.Value<float>() ?? 0;
      var minimumQuantity = (decimal)Math.Abs(Math.Floor(Math.Log(stepSize, 10)));

      return new Crypto(this, item?["baseAsset"]?.Value<string>() ?? "",
                item?["quoteAsset"]?.Value<string>() ?? "", item?["baseAssetPrecision"]?.Value<int>() ?? 0,
                item?["quoteAssetPrecision"]?.Value<int>() ?? 0, item?["symbol"]?.Value<string>() ?? "",
                minimumQuantity);
    }

    public async Task<IEnumerable<Crypto>> GetCryptoCurrencies()
    {
      string cryptoResponses = await Service.SendPublicAsync(HttpMethod.Get, "/api/v3/exchangeInfo");

      JObject cryptoJsonArray = JObject.Parse(cryptoResponses);
      var symbolArr = cryptoJsonArray?["symbols"]?.Value<JArray>();
      if (symbolArr is null) return new List<Crypto>();

      return symbolArr.Select(arr => 
          new Crypto(this, arr?["baseAsset"]?.Value<string>() ?? "",
          arr?["quoteAsset"]?.Value<string>() ?? "", arr?["baseAssetPrecision"]?.Value<int>() ?? 0,
          arr?["quoteAssetPrecision"]?.Value<int>() ?? 0, arr?["symbol"]?.Value<string>() ?? ""));
    }

    public async Task<CoreStatus> BuyAsset(Crypto crypto, decimal price, decimal quantity = 0.00M)
    {
      Dictionary<string, object> stopLossParameter = new()
      {
        { "symbol", crypto.PairName },
        { "side", "BUY" },
        { "type", "MARKET" },
        { "quantity", crypto.QuoteMinimumQuantity },
      };

      var buyResponse = await Service.SendSignedAsync
        (HttpMethod.Post, "/api/v3/order",
        stopLossParameter);

      JObject buyJson = JObject.Parse(buyResponse);

      string msg = buyJson?["msg"]?.Value<string>() ?? "Success";

      Dictionary<string, CoreStatus> buyActivities = new Dictionary<string, CoreStatus>()
      {
        {"Timestamp for this request was 1000ms ahead of the server's time.", CoreStatus.OUT_OF_SYNC},
        {"Stop loss orders are not supported for this symbol", CoreStatus.BUY_UNSUCCESSFUL },
        {"Account has insufficient balance for requested action." , CoreStatus.INSUFFICIENT_FUNDS},
        {"Success" , CoreStatus.BUY_SUCCESSFUL},
      };

      return buyActivities.ContainsKey(msg)
        ? buyActivities[msg]
        : CoreStatus.UNKNOWN_ERROR;
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
