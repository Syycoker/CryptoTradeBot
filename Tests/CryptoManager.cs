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
    List<ICore> Cores = CoreFactory.GetCores().ToList();

    public List<Crypto> AllCryptos = new List<Crypto>();
    public List<Crypto> BinanceCryptos = new List<Crypto>();
    public List<Crypto> CoinbaseCryptos = new List<Crypto>();

    public CryptoManager()
    {
      var binanceCore = Cores[0];
      var coinbaseCore = Cores[1];

      BinanceCryptos.Add(new Crypto(binanceCore, string.Empty, string.Empty, 0, 0));
      CoinbaseCryptos.Add(new Crypto(coinbaseCore, "BTC", "USD", 7, 7));
      BinanceCryptos.Add(new Crypto(binanceCore, "BTC", "USD", 6, 8));
      CoinbaseCryptos.Add(new Crypto(coinbaseCore, "LTC", "USD", 7, 7));
      BinanceCryptos.Add(new Crypto(binanceCore, "LTC", "USD", 7, 7));
      CoinbaseCryptos.Add(new Crypto(coinbaseCore, string.Empty, string.Empty, 0, 0));

      AllCryptos.AddRange(BinanceCryptos);
      AllCryptos.AddRange(CoinbaseCryptos);

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
