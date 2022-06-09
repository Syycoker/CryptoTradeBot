using FractionalCryptoBot.Configuration;
using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace FractionalCryptoBot.Exceptions
{
  /// <summary>
  /// An exception to handle an invalid file so that the program can proceed.
  /// </summary>
  public class InvalidAuthenticationException : Exception
  {
    #region Constants
    const string AUTHENTICATION = "Authentication";
    const string DEFAULT_API = "DEFAULT_API";
    const string DEFAULT_API_TEST = "DEFAULT_API_TEST";
    const string AUTH_KEY = "AUTH_KEY";
    const string AUTH_SECRET = "AUTH_SECRET";
    const string AUTH_PASSPHRASE = "AUTH_PASSPHRASE";
    const string AUTH_URL = "AUTH_URL";
    const string SOCKET_URL = "SOCKET_URL";
    const string VALUE = "value";
    #endregion
    #region Members
    public string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    #endregion
    #region Constructor
    /// <summary>
    /// Creates a default ".xml" file which provides the correct structure for the "AuthenticationConfig" Class to parse correctly.
    /// </summary>
    public InvalidAuthenticationException()
    {
      CreateXmlFile();
    }

    /// <summary>
    /// Creates a default ".json" file which provides the correct structure for the "AuthenticationConfig" Class to parse correctly.
    /// </summary>
    public InvalidAuthenticationException(string filePath)
    {
      CreateJsonFile(filePath);
    }
    #endregion
    #region Private
    /// <summary>
    /// Creates a json file that contains all the authenitcation for different exchanges.
    /// </summary>
    /// <param name="filePath"></param>
    private void CreateJsonFile(string filePath)
    {
      ExchangeAuth newExchangeAuth = new()
      {
        Exchanges = new()
        {
          new ExchangeAuthentication(null)
          {
            Exchange = "BINANCE",
            ApiKey = "api_key_example",
            ApiSecret = "api_secret_example",
            ApiPass = "api_pass_example"
          }
        }
      };

      string serialisedObj = 
        JsonConvert.SerializeObject(newExchangeAuth,
        Formatting.Indented);

      File.WriteAllLines(filePath, new string[] { serialisedObj });
    }

    /// <summary>
    /// Creates an xml file that contains all the authentication for different exchanges.
    /// </summary>
    private void CreateXmlFile()
    {
      try
      {
        // Create a file name for the user's xml file.
        const string authenticationXmlFileName = "UserAuthentication.xml";
        string userFilePath = DesktopPath + "\\" + authenticationXmlFileName; 

        List<string> lines = new();
        lines.Add("<?xml version=" + '\u0022' + "1.0" + '\u0022' + " encoding=" + '\u0022' + "utf-8" + '\u0022' + " ?>");
        lines.Add($"<{AUTHENTICATION}>");
        lines.Add($"</{AUTHENTICATION}>");
        // To make it a valid ".xml" file so the file knows how to encode/decode the file appropriately.
        File.WriteAllLines(userFilePath, lines);

        // To make sure the thread creating the file isn't still creating before we load it.
        Thread.Sleep(10);
        XmlDocument doc = new();
        doc.Load(userFilePath);

        // Now let's create an empty xml file for our user!
        XmlElement? docElement = doc.DocumentElement;

        if (docElement is null)
          throw new Exception(nameof(docElement) + " was null.");

        XmlElement defaultApi = doc.CreateElement(DEFAULT_API);
        XmlElement defaultApiTest = doc.CreateElement(DEFAULT_API_TEST);

        AppendAttributes(defaultApi);
        AppendAttributes(defaultApiTest);

        docElement.AppendChild(defaultApi);
        docElement.AppendChild(defaultApiTest);
        doc.AppendChild(docElement);


        doc.Save(userFilePath);
      }
      catch
      {
        // Swallow Exception
      }
    }

    /// <summary>
    /// If successful, return a list of the now added elements.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    private List<XmlNode>? AppendAttributes(XmlElement element)
    {
      try
      {
        List<XmlNode> attributes = new();

        // Get the document owner
        XmlDocument doc = element.OwnerDocument;

        XmlElement authKeyNode = doc.CreateElement(AUTH_KEY);
        XmlElement authSecretNode = doc.CreateElement(AUTH_SECRET);
        XmlElement authPassphraseNode = doc.CreateElement(AUTH_PASSPHRASE);
        XmlElement authUrlNode = doc.CreateElement(AUTH_URL);
        XmlElement authSocketNode = doc.CreateElement(SOCKET_URL);

        authKeyNode.SetAttribute(VALUE, "YourApiKey");
        authSecretNode.SetAttribute(VALUE, "YourSecret");
        authPassphraseNode.SetAttribute(VALUE, "YourPassphrase");
        authUrlNode.SetAttribute(VALUE, "TheUri");
        authSocketNode.SetAttribute(VALUE, "TheSocketUri");

        element.AppendChild(authKeyNode);
        element.AppendChild(authSecretNode);
        element.AppendChild(authPassphraseNode);
        element.AppendChild(authUrlNode);

        attributes.Add(authKeyNode);
        attributes.Add(authSecretNode);
        attributes.Add(authPassphraseNode);
        attributes.Add(authUrlNode);
        attributes.Add(authSocketNode);

        return attributes;
      }
      catch
      {
        // Throw exception up to notify that the operation was unsuccessful.
        return null;
      }
    }
    #endregion
  }
}
