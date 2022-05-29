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
  /// Class to test if the methods in the 'SharedCrypto' DTO work as intended.
  /// </summary>
  public class SharedCryptoTestClass
  {
    readonly SharedCrypto sharedCrypto;

    public SharedCryptoTestClass()
    {
      sharedCrypto = new SharedCryptoManager().CreateSharedCrypto().ToList()[0];
    }

    [Fact]
    public void Gets_The_Lowest_Bidding_Crypto()
    {
      var lowestAsset = sharedCrypto.GetLowestBaseBiddingAsset();
      var lowestPrice = lowestAsset?.BaseBiddingPrice;

      var collectionWithoutLowestAsset =
        new SharedCrypto(sharedCrypto
        .Cryptos
        .Where(crypto => 
        !crypto.Equals(lowestAsset)));

      var lowestAssetInNewCollection = collectionWithoutLowestAsset.GetLowestBaseBiddingAsset();

      var lowestAmmenededPrice = lowestAssetInNewCollection?.BaseBiddingPrice;

      Assert.True(lowestPrice < lowestAmmenededPrice);
    }

    [Fact]
    public void Gets_The_Highest_Bidding_Crypto()
    {
      var highestAsset = sharedCrypto.GetHighestBaseBiddingAsset();
      var highestPrice = highestAsset?.BaseBiddingPrice;

      var collectionWithoutHighestAsset =
        new SharedCrypto(sharedCrypto
        .Cryptos
        .Where(crypto =>
        !crypto.Equals(highestAsset)));

      var highestAssetInNewCollection = collectionWithoutHighestAsset.GetHighestBaseBiddingAsset();

      var highestAmmenededPrice = highestAssetInNewCollection?.BaseBiddingPrice;

      Assert.True(highestPrice > highestAmmenededPrice);
    }
  }
}