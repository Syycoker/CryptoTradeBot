using FractionalCryptoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
  public class SharedCryptoManager
  {
    public IEnumerable<SharedCrypto> CreateSharedCrypto()
    {
      return new List<SharedCrypto>()
      {
        new SharedCrypto(new CryptoManager().BinanceCryptos),
        new SharedCrypto(new CryptoManager().CoinbaseProCryptos)
      };
    }
  }
}
