using FractionalCryptoBot.Configuration;
using System.IO;
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
      AuthStub = new AuthenticationStub()
        .WithDefault()
        .Build();
    }

    [Fact]
    public void Authentication_was_created()
    {
      Assert.NotNull(AuthStub);
    }

    [Theory]
    [InlineData(false, "https://github.com/Syycoker", "https://github.com/Syycoker", "api_key_example", "api_secret_example", "api_pass_example")]
    [InlineData(true, "https://github.com/Syycoker", "https://github.com/Syycoker", "api_key_sandbox_example", "api_secret_sandbox_example", "api_pass_sandbox_example")]
    public void Authentication_returns_correct_values(bool sandbox, string uri,
      string websocket, string key, string secret, string pass)
    {
      AuthStub.Authentication.SandboxMode = sandbox;

      Assert.Equal(uri, AuthStub?.Authentication?.Uri);
      Assert.Equal(websocket, AuthStub?.Authentication?.WebsocketUri);
      Assert.Equal(key, AuthStub?.Authentication?.Key);
      Assert.Equal(secret, AuthStub?.Authentication?.Secret);
      Assert.Equal(pass, AuthStub?.Authentication?.Pass);
    }
  }
}