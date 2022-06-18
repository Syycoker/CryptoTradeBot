using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace FractionalCryptoBot.Configuration
{
  public class ExchangeAuthentication : IAuthentication
  {
    [JsonPropertyName("Exchange")]
    public string Exchange { get; set; } = string.Empty;

    [JsonPropertyName("ApiKey")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonPropertyName("ApiSecret")]
    public string ApiSecret { get; set; } = string.Empty;

    [JsonPropertyName("ApiPass")]
    public string ApiPass { get; set; } = string.Empty;

    [JsonPropertyName("ApiKeySandbox")]
    public string ApiKeySandbox { get; set; } = string.Empty;

    [JsonPropertyName("ApiSecretSandbox")]
    public string ApiSecretSandbox { get; set; } = string.Empty;

    [JsonPropertyName("ApiPassSandbox")]
    public string ApiPassSandbox { get; set; } = string.Empty;
  }
}