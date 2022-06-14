using FractionalCryptoBot.Configuration;
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
      Authentication = new AuthenticationStub(false);
    }

    [Fact]
    public void Authentication_should_not_be_null()
    {
      Assert.True(Authentication is not null);
    }

    [Fact]
    public void Authentication_returns_correct_value()
    {
      Assert.Equal("api_key_example", Authentication?.Authentication?.ApiKey);
      Assert.Equal("api_secret_example", Authentication?.Authentication?.ApiSecret);
      Assert.Equal("api_pass_example", Authentication?.Authentication?.ApiPass);
    }

    [Fact]
    public void Authentication_returns_correct_value_Sandbox_Mode()
    {
      var newStub = new AuthenticationStub(true);

      Assert.Equal("api_key_sandbox_example", newStub?.Authentication?.ApiKey);
      Assert.Equal("api_secret_sandbox_example", newStub?.Authentication?.ApiSecret);
      Assert.Equal("api_pass_sandbox_example", newStub?.Authentication?.ApiPass);
    }
  }
}