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
    public readonly AuthenticationStub AuthStub;

    public AuthenticationTests()
    {
      AuthStub = new AuthenticationStub();
    }

    [Fact]
    public void Authentication_should_not_be_null()
    {
      Assert.True(AuthStub is not null);
    }

    [Fact]
    public void Authentication_returns_correct_value()
    {
      Assert.Equal("api_key_example", AuthStub?.Authentication?.ApiKey);
      Assert.Equal("api_secret_example", AuthStub?.Authentication?.ApiSecret);
      Assert.Equal("api_pass_example", AuthStub?.Authentication?.ApiPass);
    }

    [Fact]
    public void Authentication_returns_correct_value_Sandbox_Mode()
    {
      Assert.Equal("api_key_sandbox_example", AuthStub?.Authentication?.ApiKeySandbox);
      Assert.Equal("api_secret_sandbox_example", AuthStub?.Authentication?.ApiSecretSandbox);
      Assert.Equal("api_pass_sandbox_example", AuthStub?.Authentication?.ApiPassSandbox);
    }
  }
}