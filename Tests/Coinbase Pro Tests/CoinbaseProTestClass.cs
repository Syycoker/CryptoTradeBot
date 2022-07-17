using FractionalCryptoBot;
using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Enumerations;
using FractionalCryptoBot.Models;
using FractionalCryptoBot.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tests.Authentication_Tests;
using Xunit;

namespace Tests
{
  /// <summary>
  /// Class to test if the methods in the 'Crypto' DTO work as intended.
  /// </summary>
  public class CoinbaseProTestClass
  {
    readonly ICore _coinbasePro;
    readonly AuthenticationStub _authenticationStub;

    public CoinbaseProTestClass()
    {
      _authenticationStub = new AuthenticationStub()
        .UsingExchange(Marketplaces.COINBASE_PRO)
        .Build();

      _coinbasePro = new CBProCore(new LoggerFactory().CreateLogger<ICore>());

      _coinbasePro.Service.Authentication = _authenticationStub.Authentication;
    }

    [Fact]
    public void Coinbase_Pro_Core_Is_Not_Null()
      => Assert.NotNull(_coinbasePro);

    [Fact]
    public async Task Can_Make_Active_Call()
      => Assert.True(await _coinbasePro.ActiveService());

    [Theory]
    [InlineData("ETH-USD", "ETH","USD")]
    [InlineData("BTC-USD", "BTC","USD")]
    [InlineData("USDT-USD", "USDT","USD")]
    public async Task Can_get_a_collection_of_cryptocurrency(string cryptoName, params string[] expectedCosignments)
    {
      var crypto = await _coinbasePro.GetCryptoCurrency(cryptoName);

      Assert.True(crypto?.BaseName.Equals(expectedCosignments[0]));
      Assert.True(crypto?.QuoteName.Equals(expectedCosignments[1]));
      Assert.True(crypto?.PairName.Equals(cryptoName));
    }

    [Fact]
    public async Task Can_Get_Collection_Of_Cryptocurrency()
    {
      var collection = await _coinbasePro.GetCryptoCurrencies();
      Assert.NotNull(collection);
      Assert.True(collection.Count() > 0);
    }
  }
}