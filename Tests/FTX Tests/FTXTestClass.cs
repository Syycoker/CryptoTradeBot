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
  public class FTXTestClass
  {
    readonly ICore _ftxCore;
    readonly AuthenticationStub _authenticationStub;

    public FTXTestClass()
    {
      _authenticationStub = new AuthenticationStub()
        .UsingExchange(Marketplaces.FTX)
        .Build();

      _ftxCore = CoreFactory.GetCore(Marketplaces.FTX)
        ?? new BinanceCore(new LoggerFactory().CreateLogger<ICore>());

      _ftxCore.Service.Authentication = _authenticationStub.Authentication;
    }

    [Fact]
    public void FTX_Core_Is_Not_Null() => Assert.NotNull(_ftxCore);

    [Fact]
    public async Task Can_Make_Active_Call()
    {
      var isActive = await _ftxCore.ActiveService();
      Assert.True(isActive);
    }

    [Fact]
    public async Task Can_Start_Stream()
    {
      var crypto = new Crypto(_ftxCore, "ETH", "BTC", 0, 0, "ETHBTC");
      if (crypto is null) return;

      await crypto.RunStream();

      Thread.Sleep(15000);

      Assert.True(crypto.BidPrice != decimal.Zero);
      Assert.True(crypto.BidQty != decimal.Zero);
      Assert.True(crypto.AskPrice != decimal.Zero);
      Assert.True(crypto.AskQty != decimal.Zero);
    }

    [Theory]
    [InlineData("ETHBTC", "eth", "btc")]
    [InlineData("BTCUSDT", "btc", "usdt")]
    public async Task Can_Get_A_Cryptocurrency(string cryptoName, params string[] expectedCosignments)
    {
      _authenticationStub.Authentication.SandboxMode = true;

      var crypto = await _ftxCore.GetCryptoCurrency(cryptoName);
      Assert.NotNull(crypto);
      Assert.True(crypto?.BaseName.ToLower().Equals(expectedCosignments[0]));
      Assert.True(crypto?.QuoteName.ToLower().Equals(expectedCosignments[1]));
      Assert.True(crypto?.QuoteMinimumQuantity != decimal.Zero);
    }

    [Fact]
    public async Task Can_Get_Collection_Of_Cryptocurrency()
    {
      _authenticationStub.Authentication.SandboxMode = true;

      var collection = await _ftxCore.GetCryptoCurrencies();
      Assert.NotNull(collection);
      Assert.True(collection.Count() > 0);
      Assert.Contains(collection, c => c.PairName.Equals("ETHBTC"));
    }

    [Fact]
    public async Task Can_Buy_Asset()
    {
      _authenticationStub.Authentication.SandboxMode = true;

      Crypto? crypto = await _ftxCore.GetCryptoCurrency("ETHBTC");
      if (crypto is null) return;
      crypto.SetBidQty(1.00m);

      var operationStatus = await crypto.BuyAsset();

      Assert.Equal(CoreStatus.BUY_SUCCESSFUL, operationStatus);
    }
  }
}