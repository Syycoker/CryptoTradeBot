namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// A DTO for cryptocurrencies.
  /// </summary>
  public class Crypto
  {
    /// <summary>
    /// The name of the crptocurrency as its short version of its name, i.e. 'BTC' -> 'Bitcoin'.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The precision of the asset if/when placing a bid for it on a marketplace.
    /// </summary>
    public readonly int Precision = 0;

    /// <summary>
    /// Standard constructor for constructor injection.
    /// </summary>
    /// <param name="name">The name of the cryptocurrency as a 3-5 letter shorthand.</param>
    /// <param name="precision">The precision of the asset.</param>
    public Crypto(string name, int precision)
    {
      Name = name;
      Precision = precision;
    }
  }
}
