using FractionalCryptoBot.Cores;
using Moq;
using System.Net.Http;
using Xunit;

namespace Tests
{
  /// <summary>
  /// Class to test the validity of each class that implements 'ICore'.
  /// </summary>
  public class CoreSystemTests
  {
    [Fact]
    public void Do_The_Services_In_Each_Core_Have_A_Stable_Server_Connection()
    {
      var serviceMock = new Mock<ICore>();

      // Hmm not too sure how to test, but what i'll d is send a request to the service endpoint for each marketplace
      // serviceMock.Setup(s => s.Service.SendPublicAsync(HttpMethod.Get, "").Result));
    }
  }
}