using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Cores
{
  public sealed class BinanceCore : ICore
  {
    #region Private Members
    private int _numberOfCryptoCurrencies = 0;
    private decimal _totalPriceChange = 0;
    private double _marketplaceAverage = 0;
    private List<Crypto> _cryptoCurrencies = new();
    #endregion
    #region Public Members
    public int NumberOfCryptoCurrencies { get => _numberOfCryptoCurrencies; private set => _numberOfCryptoCurrencies = value; }
    public decimal TotalPriceChange { get => _totalPriceChange; private set => _totalPriceChange = value; }
    public double MarketplaceAverage { get => _marketplaceAverage; private set => _marketplaceAverage = value; }
    public ILogger Log { get; init; }
    public IHttpService Service { get; set; }
    public IEnumerable<Crypto> Cryptocurrencies { get => _cryptoCurrencies; set => _cryptoCurrencies = new List<Crypto>(value); }
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

    public CoreStatus BuyAsset(Crypto crypto, double amount)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Crypto> GetCryptoCurrencies()
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Crypto> GetPerformantCrypto()
    {
      throw new NotImplementedException();
    }

    public CoreStatus SellAsset(Crypto crypto, double amount)
    {
      throw new NotImplementedException();
    }

    public CoreStatus TransferAssetToColdWallet(Crypto crypto, int walletId)
    {
      throw new NotImplementedException();
    }

    public CoreStatus TransferAssetToExchange(Crypto crypto, string walletId)
    {
      throw new NotImplementedException();
    }

    public JObject GetAsset(string crypto)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
