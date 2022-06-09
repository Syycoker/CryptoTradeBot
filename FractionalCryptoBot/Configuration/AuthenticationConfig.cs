using FractionalCryptoBot.Exceptions;
using System.Net;
using System.Xml;
using FractionalCryptoBot.Enumerations;
using Newtonsoft.Json.Linq;

namespace FractionalCryptoBot.Configuration
{
  public static class AuthenticationConfig
  {
    #region Constants
    public const string API_NAME = "DEFAULT_API";
    public const string API_TEST_NAME = "DEFAULT_API_TEST";
    public const string API_KEY = "API_KEY";
    public const string API_SECRET = "API_SECRET";
    public const string API_PASS = "API_PASSPHRASE";
    public const string API_URL = "API_URL";
    public const string SOCKET_URL = "SOCKET_URL";
    #endregion
    #region Public
    /// <summary>
    ///  Checks if Authentication Config has been initialised.
    /// </summary>
    public static bool Initialised { get; set; }

    /// <summary>
    /// To check wheter the authentication strings should be from the sandbox api or not.
    /// </summary>
    public static bool SandBoxMode = false;
    #endregion
    #region Initialisation
    /// <summary>
    /// Attempts to parse the authentication for a .json file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidAuthenticationException"></exception>
    public static bool Initialise(string filePath)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      if (string.IsNullOrEmpty(filePath)) return false;

      try
      {
        if (!File.Exists(filePath)) throw new InvalidAuthenticationException(filePath);
      }
      catch
      {
        // Swallow exception
      }

      var authentications = JObject.Parse(File.ReadAllText(filePath));

      var exchanges = authentications["Exchanges"]?.Value<JArray>();

      if (exchanges is null) throw new InvalidAuthenticationException();

      foreach (JObject exchange in exchanges)
      {
        ExchangeAuthentication newAuth = new(exchange);
        Authentications.Add(newAuth.Exchange, newAuth);
      }

      return true;
    }

    /// <summary>
    /// Attempts to return the authentications for an exchange.
    /// </summary>
    /// <param name="marketplace"></param>
    /// <returns></returns>
    public static IAuthentication? GetAuthentication(Marketplaces marketplace)
    {
      return Authentications[marketplace.ToString()];
    }
    #endregion
    #region Private
    private static Dictionary<string, IAuthentication> Authentications = new();

    /// <summary>
    /// The file address for the authentication file.
    /// </summary>
    private static string GetAuthenticationFilePath() { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "UserAuthentication.xml"; }
    #endregion
  }
}
