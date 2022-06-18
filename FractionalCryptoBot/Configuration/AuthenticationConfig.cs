using FractionalCryptoBot.Exceptions;
using System.Net;
using System.Xml;
using FractionalCryptoBot.Enumerations;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FractionalCryptoBot.Configuration
{
  public static class AuthenticationConfig
  {
    #region Public
    /// <summary>
    ///  Checks if Authentication Config has been initialised.
    /// </summary>
    public static bool Initialised { get; set; }

    public static bool SandboxMode { get; set; }
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

      if (string.IsNullOrEmpty(filePath)) filePath = GetAuthenticationFilePath();

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

      Authentications.Clear();

      foreach (JObject exchange in exchanges)
      {
        var exchangeObj = JsonConvert.DeserializeObject<ExchangeAuthentication>(exchange.ToString(Newtonsoft.Json.Formatting.None));

        if (exchangeObj is null) continue;

        Authentications.Add(exchangeObj.Exchange, exchangeObj);
      }

      Initialised = true;
      return true;
    }

    /// <summary>
    /// Attempts to return the authentications for an exchange.
    /// </summary>
    /// <param name="marketplace"></param>
    /// <returns></returns>
    public static IAuthentication? GetAuthentication(Marketplaces marketplace)
    {
      if (!Initialised) Initialise(string.Empty);

      return Authentications.ContainsKey($"{marketplace}")
      ? Authentications[$"{marketplace}"]
      : new ExchangeAuthentication();
    }
      
    /// <summary>
    /// The file address for the authentication file.
    /// </summary>
    public static string GetAuthenticationFilePath() { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "FractionalCryptoBotAuthentication.json"; }
    #endregion
    #region Private
    /// <summary>
    /// Dictionary that contains and authentication of all marketplace information that are account specific.
    /// </summary>
    private static Dictionary<string, IAuthentication> Authentications = new();
    #endregion
  }
}
