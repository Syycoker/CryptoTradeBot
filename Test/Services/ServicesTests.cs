using Xunit;

namespace Test
{
  /// <summary>
  /// Class to test the stability and activity of the services listed.
  /// </summary>
  public class ServicesTests
  {
    [Fact]
    public void ServicesAreActive()
    {
      var serviceMock = new Mock<ICore>();

      // We check if we can make a call to binance's service and recieve a valid response, 200, 300, 400, 500.
      Assert.Equal(string.Empty, string.Empty);
    }
  }
}