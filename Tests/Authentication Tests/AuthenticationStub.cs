using FractionalCryptoBot.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Authentication_Tests
{
  public class AuthenticationStub
  {
    public readonly IAuthentication Authentication;

    public AuthenticationStub()
    {
      AuthenticationConfig.Initialise("C:\\Users\\Sylas Coker\\Documents\\newAuthen.json");
      Authentication = AuthenticationConfig.GetAuthentication(FractionalCryptoBot.Enumerations.Marketplaces.BINANCE);
    }
  }
}
