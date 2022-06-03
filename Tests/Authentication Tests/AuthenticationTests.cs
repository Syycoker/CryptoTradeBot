using Tests.Authentication_Tests;
using Xunit;

namespace Tests
{
  /// <summary>
  /// Class to test the validity of each class that implements 'ICore'.
  /// </summary>
  public class AuthenticationTests
  {
    public readonly AuthenticationStub Authentication;

    public AuthenticationTests()
    {
      Authentication = new AuthenticationStub();
    }

    [Fact]
    public void Authentication_returns_correct_value()
    {
      Assert.Equal("sylasIsTheBest", Authentication.Authentication.ApiKey);
      Assert.Equal("sylasIsTheBest", Authentication.Authentication.ApiSecret);
      Assert.Equal("sylasIsTheBest", Authentication.Authentication.ApiPass);
    }
  }
}