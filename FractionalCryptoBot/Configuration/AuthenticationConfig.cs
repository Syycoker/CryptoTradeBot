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
    public static Mutex Mutex { get; set; } = new Mutex();
    #endregion
    #region Initialisation
    /// <summary>
    /// Attempts to parse the authentication for a .json file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidAuthenticationException"></exception>
    public static bool Initialise(string filePath,
      out Dictionary<string, IAuthentication> authenticationDictionary)
    {
      Mutex.WaitOne();
      try
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

        authenticationDictionary = new();

        foreach (JObject exchange in exchanges)
        {
          var exchangeObj = JsonConvert.DeserializeObject<ExchangeAuthentication>(exchange.ToString(Newtonsoft.Json.Formatting.None));

          if (exchangeObj is null) continue;

          authenticationDictionary.Add(exchangeObj.Exchange, exchangeObj);
        }

        return true;
      }
      finally
      {
        Mutex.ReleaseMutex();
      }
    }

    /// <summary>
    /// Attempts to return the authentications for an exchange.
    /// </summary>
    /// <param name="marketplace"></param>
    /// <returns></returns>
    public static IAuthentication? GetAuthentication(Marketplaces marketplace, string filePath = "")
    {
      string path = string.IsNullOrEmpty(filePath)
        ? GetAuthenticationFilePath()
        : filePath;

      Initialise(path, out var authentication);

      return authentication.ContainsKey($"{marketplace}")
      ? authentication[$"{marketplace}"]
      : new ExchangeAuthentication();
    }
      
    /// <summary>
    /// The file address for the authentication file.
    /// </summary>
    public static string GetAuthenticationFilePath() { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "FractionalCryptoBotAuthentication.json"; }
    #endregion
  }
}
