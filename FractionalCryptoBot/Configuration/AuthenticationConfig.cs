using FractionalCryptoBot.Exceptions;
using System.Net;
using System.Xml;
using FractionalCryptoBot.Enumerations;
using Newtonsoft.Json.Linq;

namespace FractionalCryptoBot.Configuration
{
  public static class AuthenticationConfig
  {
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
    #region Static Constructor
    static AuthenticationConfig()
    {
      // Initialise(GetAuthenticationFilePath());
    }
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

      // if (string.IsNullOrEmpty(filePath)) filePath = GetAuthenticationFilePath();

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
    public static IAuthentication? GetAuthentication(Marketplaces marketplace) => Authentications[marketplace.ToString()];
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
