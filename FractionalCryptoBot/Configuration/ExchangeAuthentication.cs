using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace FractionalCryptoBot.Configuration
{
  public class ExchangeAuthentication : IAuthentication
  {
    [JsonPropertyName("Exchange")]
    public string Exchange { get; set; } = string.Empty;

    [JsonPropertyName("ApiUrl")]
    public string ApiUrl { get; set; } = string.Empty;

    [JsonPropertyName("WebsocketUrl")]
    public string WebsocketUrl { get; set; } = string.Empty;

    [JsonPropertyName("ApiKey")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonPropertyName("ApiSecret")]
    public string ApiSecret { get; set; } = string.Empty;

    [JsonPropertyName("ApiPass")]
    public string ApiPass { get; set; } = string.Empty;

    [JsonPropertyName("ApiUrlSandbox")]
    public string ApiUrlSandbox { get; set; } = string.Empty;

    [JsonPropertyName("WebsocketUrlSandbox")]
    public string WebsocketUrlSandbox { get; set; } = string.Empty;

    [JsonPropertyName("ApiKeySandbox")]
    public string ApiKeySandbox { get; set; } = string.Empty;

    [JsonPropertyName("ApiSecretSandbox")]
    public string ApiSecretSandbox { get; set; } = string.Empty;

    [JsonPropertyName("ApiPassSandbox")]
    public string ApiPassSandbox { get; set; } = string.Empty;

    [JsonIgnore]
    public bool SandboxMode { get; set; } = false;

    [JsonIgnore]
    public string Uri => SandboxMode ? ApiUrlSandbox : ApiUrl;
    [JsonIgnore]
    public string WebsocketUri => SandboxMode ? WebsocketUrlSandbox : WebsocketUrl;
    [JsonIgnore]
    public string Key => SandboxMode ? ApiKeySandbox : ApiKey;
    [JsonIgnore]
    public string Secret => SandboxMode ? ApiSecretSandbox : ApiSecret;
    [JsonIgnore]
    public string Pass => SandboxMode ? ApiPassSandbox : ApiPass;
  }
}