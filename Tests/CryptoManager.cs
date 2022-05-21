using FractionalCryptoBot;
using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
  /// <summary>
  /// A class to produce a dummy collection oF 'Crypto' collection.
  /// </summary>
  public class CryptoManager : IEnumerable<Crypto>
  {
    List<ICore> Cores = CoreFactory.GetCores().ToList();

    List<Crypto> Crypto = new List<Crypto>();

    public CryptoManager()
    {
      var binanceCore = CoreFactory.GetCore(typeof(BinanceCore));
      if (binanceCore is null) return;

      // Add an invalid crypto
      Crypto.Add(new Crypto(binanceCore, string.Empty, 1, 0.00m, 0.00m));
      Crypto.Add(new Crypto(binanceCore, "BTC", 8, 0.00m, 0.00m));
      Crypto.Add(new Crypto(binanceCore, "ETH", 7, 0.00m, 0.00m));
      Crypto.Add(new Crypto(binanceCore, "LUNA", 8, 0.00m, 0.00m));
      Crypto.Add(new Crypto(binanceCore, "XRP", 8, 0.00m, 0.00m));
      Crypto.Add(new Crypto(binanceCore, "DOGE", 8, 0.00m, 0.00m));
      Crypto.Add(new Crypto(binanceCore, "SHIB", 8, 0.00m, 0.00m));
      Crypto.Add(new Crypto(binanceCore, "SOL", 8, 0.00m, 0.00m));
    }

    public IEnumerator<Crypto> GetEnumerator()
    {
      return Crypto.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
