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
    public void Do_The_Services_In_Each_Core_Have_A_Stable_Server_Connection()
    {
      var serviceMock = new Mock<ICore>();

      // Hmm not too sure how to test, but what i'll d is send a request to the service endpoint for each marketplace
      // serviceMock.Setup(s => s.Service.SendPublicAsync(HttpMethod.Get, "").Result));
    }

    [Fact]
    /// Test to check if any of the cores in the system give an inactive state.
    public void CheckIfAllServiceCallsAreValid()
    {
      // Get all the cores in the system.
      IEnumerable<ICore> cores = CoreFactory.GetCores();

      // Get all cores that give an inactive service status.
      var anyCoresInactive = cores.Any(core => !core.ActiveService().Result);

      // We don't expect any cores to be inactive
      Assert.False(anyCoresInactive);
    }

    [Theory]
    [ClassData(typeof(CryptoManager))]
    /// <summary>
    /// Checks if the 'SharedDTO' will aggregate commonalities if they're valid...
    /// </summary>
    public void CheckIfSharedCryptoDtoWorks(int expectedItemLength, params Crypto[] cryptos)
    {
      Assert.Equal(1, cryptos.Length);
    }
  }
}