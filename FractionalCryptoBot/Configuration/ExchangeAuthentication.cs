using Newtonsoft.Json.Linq;

namespace FractionalCryptoBot.Configuration
{
  public class ExchangeAuthentication : IAuthentication
  {
    public string Exchange { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string ApiSecret { get; set; } = string.Empty;

    public string ApiPass { get; set; } = string.Empty;

    public ExchangeAuthentication(JObject token)
    {
      Exchange = token?[nameof(Exchange)]?.Value<string>() ?? string.Empty;
      ApiKey = token?[nameof(ApiKey)]?.Value<string>() ?? string.Empty;
      ApiSecret = token?[nameof(ApiSecret)]?.Value<string>() ?? string.Empty;
      ApiPass = token?[nameof(ApiPass)]?.Value<string>() ?? string.Empty;
    }
  }
}