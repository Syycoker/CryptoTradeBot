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
  public sealed class CBProCore : ICore
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
      HttpResponseMessage response = 
        await new HttpClient()
        .SendAsync(new HttpRequestMessage()
        {
          Method = HttpMethod.Get,
          RequestUri = new Uri(@"https://api.coinbase.com/v2/time"),
        });

      return response.IsSuccessStatusCode;
    }

    public async Task<Crypto?> GetCryptoCurrency(string crypto)
    {
      var response = await Service
        .SendPublicAsync(HttpMethod.Get, $"/products/{crypto}");

      var json = JObject.Parse(response);

      int basePrecision = GetPrecision(json?["base_increment"]?.Value<decimal>() ?? 0);
      int quotePrecision = GetPrecision(json?["quote_increment"]?.Value<decimal>() ?? 0);

      return new Crypto(this, json?["base_currency"]?.Value<string>() ?? "",
        json?["quote_currency"]?.Value<string>() ?? "",
        basePrecision, quotePrecision, crypto
      );
    }

    public async Task<IEnumerable<Crypto>> GetCryptoCurrencies()
    {
      var response = await Service
        .SendPublicAsync(HttpMethod.Get, $"/products");

      var json = JArray.Parse(response);

      return json?.Select(crypto => new Crypto(this, 
        json?[0]?["base_currency"]?.Value<string>() ?? "",
        json?[0]?["quote_currency"]?.Value<string>() ?? "",
        GetPrecision(json?[0]?["base_increment"]?.Value<decimal>() ?? 0),
        GetPrecision(json?[0]?["quote_increment"]?.Value<decimal>() ?? 0),
        json?[0]?["id"]?.Value<string>() ?? ""))
        ?? new List<Crypto>();
    }

    public async Task<CoreStatus> BuyAsset(Crypto crypto, decimal price, decimal quantity = 0.00M)
    {
      var parameters = new Dictionary<string, object>()
      {
        {"type", "market"},
        {"price", crypto.BidQty },
        {"size", crypto.BidQty },
        {"side", "buy" },
        {"product_id", crypto.PairName },
      };

      var response = await Service
        .SendSignedAsync(HttpMethod.Post,
        "/orders", parameters);

      JObject buyJson = JObject.Parse(response);

      string msg = buyJson?["message"]?.Value<string>() ?? "Success";

      Dictionary<string, CoreStatus> buyActivities = new Dictionary<string, CoreStatus>()
      {
        {"request timestamp expired", CoreStatus.OUT_OF_SYNC},
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
    #region Exchange Specific Methods
    private int GetPrecision(decimal number)
    {
      string num = $"{number}";
      int indexOfDecimalPoint = num.IndexOf('.');
      if (indexOfDecimalPoint == -1) indexOfDecimalPoint = 0;
      int subLength = num.Substring(indexOfDecimalPoint).Length;
      return subLength - 1;
    }
    #endregion
  }
}
