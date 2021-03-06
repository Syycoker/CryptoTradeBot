using FractionalCryptoBot;
using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
  /// <summary>
  /// Class to test if the methods in the 'Crypto' DTO work as intended.
  /// </summary>
  public class Crypto_Dto_Test_Class
  {
    readonly CryptoManager cryptoManager;

    public Crypto_Dto_Test_Class()
    {
      cryptoManager = new();
    }

    [Fact]
    public void Can_Create_A_Crypto_Collection()
    {
      var cryptos = cryptoManager.AllCryptos;
      Assert.NotNull(cryptos);
      Assert.True(cryptos.Any());
    }

    [Fact]
    public void Can_Create_A_Crypto_DTO()
    {
      var crypto = cryptoManager.AllCryptos[0];
      Assert.NotNull(crypto);
    }
  }
}