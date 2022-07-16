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
    {
      Assert.True(await _coinbasePro.ActiveService());
    }
  }
}