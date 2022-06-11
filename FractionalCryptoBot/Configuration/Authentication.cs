using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// test commit for Tomos 
namespace FractionalCryptoBot.Configuration
{
  public class Authentication
  {
    #region Constants
    const string API_KEY = "apiKey";
    const string API_SECRET = "apiSecret";
    const string API_PASS = "apiPass";
    #endregion
    #region Members
    public readonly string ApiKey;
    public readonly string ApiSecret;
    public readonly string ApiPass;
    #endregion
    #region Constructor
    public Authentication(Dictionary<string, string> authenticationKeys)
    {
      ApiKey = authenticationKeys[API_KEY];
      ApiSecret = authenticationKeys[API_SECRET];
      ApiPass = authenticationKeys[API_PASS];
    }
    #endregion
  }
}
