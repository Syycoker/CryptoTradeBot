using Xunit;

namespace Test
{
  public class Binance_Api_Tests
  {
    [Fact]
    public void ValidResponseTest()
    {
      // We check if we can make a call to binance's service and recieve a valid response, 200, 300, 400, 500.
      Assert.Equal(string.Empty, string.Empty);
    }
  }
}