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
      Assert.Equal("api_key_example", Authentication.Authentication.ApiKey);
      Assert.Equal("api_secret_example", Authentication.Authentication.ApiSecret);
      Assert.Equal("api_pass_example", Authentication.Authentication.ApiPass);
    }
  }
}