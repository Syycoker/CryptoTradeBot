using FractionalCryptoBot;
using FractionalCryptoBot.Configuration;
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

    [Theory]
    [InlineData("ETHBTC", "eth", "btc")]
    public async Task Can_Get_A_Cryptocurrency(string cryptoName, params string[] expectedCosignments)
    {
      var crypto = await binanceCore.GetCryptoCurrency(cryptoName);
      Assert.NotNull(crypto);
      Assert.True(crypto?.BaseName.ToLower().Equals(expectedCosignments[0]));
      Assert.True(crypto?.QuoteName.ToLower().Equals(expectedCosignments[1]));
    }

    [Fact]
    public async Task Can_Get_Collection_Of_Cryptocurrency()
    {
      var collection = await binanceCore.GetCryptoCurrencies();
      Assert.NotNull(collection);
      Assert.True(collection.Count() > 0);
      Assert.Contains(collection, c => c.PairName.Equals("ETHBTC"));
    }

    [Fact]
    public async Task Can_Buy_Asset_Stop_Loss() // Must use in sandbox mode / credentials
    {
      AuthenticationConfig.SandBoxMode = true;
      AuthenticationConfig.Initialise(string.Empty);
    }
  }
}