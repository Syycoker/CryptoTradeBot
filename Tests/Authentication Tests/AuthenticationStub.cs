using FractionalCryptoBot.Configuration;
using FractionalCryptoBot.Enumerations;
using System;

namespace Tests.Authentication_Tests
{
  public class AuthenticationStub
  {
    public IAuthentication Authentication { get; set; } = new ExchangeAuthentication();
    private Marketplaces _marketplace = Marketplaces.BINANCE;
    private string _path = AuthenticationConfig.GetAuthenticationFilePath();
    
    public AuthenticationStub WithDefault()
    {
      _path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "FCB_Test.json";
      return this;
    }

    public AuthenticationStub UsingExchange(Marketplaces marketplace)
    {
      _marketplace = marketplace;
      return this;
    }

    public AuthenticationStub Build()
    {
      Authentication = AuthenticationConfig.GetAuthentication(_marketplace, _path) ?? new ExchangeAuthentication();
      return this;
    }
  }
}
