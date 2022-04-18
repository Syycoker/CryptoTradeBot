using FractionalCryptoBot.Exceptions;
using System.Net;
using System.Xml;
using FractionalCryptoBot.Enumerations;

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
    public static bool SandBoxMode = ApplicationSettings.IsSandboxMode;

    /// <summary>
    /// Gets the api key for the client.
    /// </summary>
    /// <param name="service"></param>
    /// <returns>The api key for the client.</returns>
    public static string GetApiKey(Marketplaces service)
    {
      switch (service)
      {
        default:
        case Marketplaces.NONE:
          return string.Empty;

        case Marketplaces.BINANCE:
          return BinanceAuthenticationDictionary[API_KEY];

        case Marketplaces.COINBASE:
          return string.Empty;
      }
    }

    /// <summary>
    /// The api secret for the client.
    /// </summary>
    /// <param name="service"></param>
    /// <returns>The api secret for the client.</returns>
    public static string GetApiSecret(Marketplaces service)
    {
      switch (service)
      {
        default:
        case Marketplaces.NONE:
          return string.Empty;

        case Marketplaces.BINANCE:
          return BinanceAuthenticationDictionary[API_SECRET];

        case Marketplaces.COINBASE:
          return string.Empty;
      }
    }

    /// <summary>
    /// The base uri of the service.
    /// </summary>
    /// <param name="service"></param>
    /// <returns>The base uri of the chosen marketplace.</returns>
    public static string GetBaseUri(Marketplaces service)
    {
      switch (service)
      {
        default:
        case Marketplaces.NONE:
          return string.Empty;

        case Marketplaces.BINANCE:
          return BinanceAuthenticationDictionary[API_URL];

        case Marketplaces.COINBASE:
          return string.Empty;
      }
    }

    /// <summary>
    /// Returns the base websocket uri for the chosen service.
    /// </summary>
    /// <param name="service"></param>
    /// <returns>The base websocket uri of the chosen marketplace.</returns>
    public static string GetWebsocketUri(Marketplaces service)
    {
      switch (service)
      {
        default:
        case Marketplaces.NONE:
          return string.Empty;

        case Marketplaces.BINANCE:
          return BinanceAuthenticationDictionary[SOCKET_URL];

        case Marketplaces.COINBASE:
          return string.Empty;
      }
    }
    #endregion
    #region Initialisation
    /// <summary>
    /// Try to establish valid Authentication.
    /// </summary>
    /// <returns></returns>
    public static bool Initialise()
    {
      try
      {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        string filePath = GetAuthenticationFilePath();

        if (!File.Exists(filePath))
          throw new InvalidAuthenticationException();

        // Try and open a file with credentials.

        XmlDocument doc = new();
        doc.Load(filePath);
        XmlElement? docElement = doc.DocumentElement;

        if (docElement is null)
          throw new Exception(nameof(docElement) + " was null.");

        foreach (XmlNode node in docElement.ChildNodes)
        {
          if (SandBoxMode == false && node.Name.ToLower().Equals(API_NAME.ToLower())) { ParseBinanceAuthenticationNode(node); break; }
          if (SandBoxMode && node.Name.ToLower().Equals(API_TEST_NAME.ToLower())) { ParseBinanceAuthenticationNode(node); break; }
        }

        // By this stage we assume the autentication dictionary is now loaded and valid.
        // Now check if there's exactly 5 key value pairs, if so, successful, else, unsuccessful.
        return true;
      }
      catch (Exception e)
      {
        // Just check if the exception is of this type, if so, control the exception
        if (e.GetType() == typeof(InvalidAuthenticationException))
          return false;

        Initialised = false;
        return false;
      }
    }
    #endregion
    #region Private
    /// <summary>
    /// Storage to hold the 'secret' , 'key' and 'pass'.
    /// </summary>
    private static Dictionary<string, string> BinanceAuthenticationDictionary = new();

    /// <summary>
    /// Parses the authentication nodes to retrieve information and temporarily store them in the system.
    /// </summary>
    /// <param name="node"></param>
    /// <exception cref="Exception"></exception>
    private static void ParseBinanceAuthenticationNode(XmlNode node)
    {
      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (childNode is null || childNode.Attributes["value"] is null)
          throw new Exception($"Node was null.");

        string attributeValue = childNode.Attributes["value"].Value;

        if (string.IsNullOrEmpty(attributeValue))
          throw new Exception($"Attribute was null.");

        switch (childNode.Name)
        {
          case "AUTH_KEY":
            BinanceAuthenticationDictionary.Add(API_KEY, attributeValue);
            break;
          case "AUTH_SECRET":
            BinanceAuthenticationDictionary.Add(API_SECRET, attributeValue);
            break;
          case "AUTH_PASSPHRASE":
            BinanceAuthenticationDictionary.Add(API_PASS, attributeValue);
            break;
          case "AUTH_URL":
            BinanceAuthenticationDictionary.Add(API_URL, attributeValue);
            break;
          case "SOCKET_URL":
            BinanceAuthenticationDictionary.Add(SOCKET_URL, attributeValue);
            break;
        }
      }
    }

    /// <summary>
    /// The file address for the authentication file.
    /// </summary>
    private static string GetAuthenticationFilePath() { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "UserAuthentication.xml"; }
    #endregion
  }
}
