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
    /// <summary>
    /// Instantiating cores for the system tests.
    /// </summary>
    IEnumerable<ICore> Cores = CoreFactory.GetCores();

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