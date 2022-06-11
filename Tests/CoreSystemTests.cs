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
  /// Class to test the validity of each class that implements 'ICore'.
  /// </summary>
  public class CoreSystemTests
  {
    [Fact]
    public void Check_If_Binance_Service_Is_Active()
    {
      var bCore = CoreFactory.GetCore(FractionalCryptoBot.Enumerations.Marketplaces.BINANCE);
      Assert.True(bCore?.ActiveService().Result);
    }

    [Fact]
    public void Check_If_CoinbasePro_Service_Is_Active()
    {
      var cbCore = CoreFactory.GetCore(FractionalCryptoBot.Enumerations.Marketplaces.COINBASE_PRO);
      Assert.True(cbCore?.ActiveService().Result);
    }

    [Fact]
    /// Test to check if any of the cores in the system give an inactive state.
    public void Check_If_All_Services_Instantiated_Are_Active()
    {
      // Get all the cores in the system.
      IEnumerable<ICore> cores = CoreFactory.GetCores();

      // Get all cores that don't give an inactive service status.
      var anyCoresInactive = cores.Any(core => !core.ActiveService().Result);

      // We don't expect any cores to be inactive
      Assert.False(anyCoresInactive);
    }
  }
}