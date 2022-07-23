using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FractionalCryptoBot
{
  public class FTXCore : ICore
  {
    #region Public Members
    public int NumberOfCryptoCurrencies { get; set; }

    public decimal TotalPriceChange { get; set; }

    public decimal MarketplaceAverage { get; set; }

    public decimal AverageVolume { get; set; }

    public decimal MakerFee { get; set; }
    public decimal TakerFee { get; set; }

    public ILogger Log { get; set; }

    public IHttpService Service { get; set; }
    #endregion
    #region Constructor
    public FTXCore(ILogger logger)
    {
      // Setting the logger and the service to make http calls to the respectve service's endpoints.
      Log = logger;
      Service = new FTXService(logger);
    }
    #endregion
    #region Public Methods
    public async Task<bool> ActiveService()
    {
      var parameters = new Dictionary<string, object>()
      {
        { "days", 1 }
      };

      var responseStr = await 
        Service.SendSignedAsync(HttpMethod.Get,
        "/api/stats/latency_stats", parameters);

      var responseObj = JObject.Parse(responseStr);
      if (responseObj is null) return false;

      return (responseObj?["success"]?.Value<bool>() ?? false);
    }

    public Task<CoreStatus> BuyAsset(Crypto crypto, decimal price, decimal quantity = 0.00M)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Crypto>> GetCryptoCurrencies()
    {
      throw new NotImplementedException();
    }

    public Task<Crypto?> GetCryptoCurrency(string crypto)
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