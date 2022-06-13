using FractionalCryptoBot;
using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
  /// <summary>
  /// Class to test if the methods in the 'Crypto' DTO work as intended.
  /// </summary>
  public class BinanceTestClass
  {
    readonly ICore binanceCore;

    public BinanceTestClass()
    {
      binanceCore = new BinanceCore(new LoggerFactory().CreateLogger<ICore>());
    }

    [Fact]
    public void Binance_Core_Is_Not_Null() => Assert.NotNull(binanceCore);

    [Fact]
    public async Task Can_Make_Active_Call()
    {
      var isActive = await binanceCore.ActiveService();
      Assert.True(isActive);
    }

    [Fact]
    public async Task Can_Get_Cryptocurrency()
    {
      var crypto = await binanceCore.GetCryptoCurrency("ETHBTC");
      Assert.NotNull(crypto);
      Assert.True(crypto?.BaseName.ToLower().Equals("eth"));
      Assert.True(crypto?.QuoteName.ToLower().Equals("btc"));
    }
  }
}