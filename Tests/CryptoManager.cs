using FractionalCryptoBot;
using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
  /// <summary>
  /// A class to produce a dummy collection oF 'Crypto' collection.
  /// </summary>
  public class CryptoManager
  {
    public List<Crypto> AllCryptos = new List<Crypto>();
    public List<Crypto> BinanceCryptos = new List<Crypto>();
    public List<Crypto> CoinbaseProCryptos = new List<Crypto>();

    public CryptoManager()
    {
      var binanceCore = CoreFactory.GetCore(FractionalCryptoBot.Enumerations.Marketplaces.BINANCE);
      var coinbaseCore = CoreFactory.GetCore(FractionalCryptoBot.Enumerations.Marketplaces.COINBASE_PRO);

      if (binanceCore is null || coinbaseCore is null) return;

      BinanceCryptos.Add(new Crypto(binanceCore, string.Empty, string.Empty, 0, 0));
      CoinbaseProCryptos.Add(new Crypto(coinbaseCore, "BTC", "USD", 7, 7));
      BinanceCryptos.Add(new Crypto(binanceCore, "BTC", "USD", 6, 8));
      CoinbaseProCryptos.Add(new Crypto(coinbaseCore, "LTC", "USD", 7, 7));
      BinanceCryptos.Add(new Crypto(binanceCore, "LTC", "USD", 7, 7));
      CoinbaseProCryptos.Add(new Crypto(coinbaseCore, string.Empty, string.Empty, 0, 0));

      AllCryptos.AddRange(BinanceCryptos);
      AllCryptos.AddRange(CoinbaseProCryptos);

      AllCryptos = SetBiddingPrices(AllCryptos).ToList();
    }

    private IEnumerable<Crypto> SetBiddingPrices(IEnumerable<Crypto> cryptos)
    {
      Random rand = new Random();

      foreach (var crypto in cryptos)
        crypto.SetBaseBiddingPrice(rand.Next(0, 100));

      return cryptos;
    }
  }
}
