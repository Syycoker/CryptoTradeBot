using FractionalCryptoBot.Cores;
using FractionalCryptoBot.Enumerations;
using Microsoft.Extensions.Logging;

namespace FractionalCryptoBot
{
  /// <summary>
  /// The class that manage's all the cores to make up the main business logic.
  /// </summary>
  public static class CoreFactory
  {
    /// <summary>
    /// A logger factory object to create a logger for each instance of a core,
    /// the logger will propogate donw to the core's respective service that it will be using.
    /// </summary>
    private static LoggerFactory LoggerFactory { get; set; } = new();

    /// <summary>
    /// Get all cores that have been instantiated by the system.
    /// </summary>
    /// <returns>A collection of cores to be used by the system.</returns>
    public static IEnumerable<ICore> GetCores()
    {
      // Only instantiate services which you explicitly want to run...
      // Get an instance of each service.
      BinanceCore _binanceCore = new(LoggerFactory.CreateLogger<ICore>());
      CBProCore _coinbaseProCore = new(LoggerFactory.CreateLogger<ICore>());

      // Return the collection of services where their exhcnage's are currently active...
      return new List<ICore>() 
      { _binanceCore, _coinbaseProCore }
      .Where(core => core.ActiveService().Result);
    }

    /// <summary>
    /// Get a core based on the type requested.
    /// </summary>
    /// <param name="core"></param>
    /// <returns>The core requested if valid, else null.</returns>
    /// <exception cref="Exception">Unable to identify 'core' type.</exception>
    public static ICore? GetCore(Type core)
    {
      // Check if the type implements ICore...
      if (core is not ICore) throw new Exception(string.Format("Unable to identify type, please specify a type which implements type '0'.", nameof(ICore)));

      // Return the core requested.
      if (typeof(BinanceCore) == core.GetType()) return new BinanceCore(LoggerFactory.CreateLogger<ICore>());
      if (typeof(CBProCore) == core.GetType()) return new CBProCore(LoggerFactory.CreateLogger<ICore>());

      // Should not reach this point, if it does no test case was created for the core requested, add it.
      return null;
    }

    /// <summary>
    /// Get a specific core based on the marketplaces enum.
    /// </summary>
    /// <param name="marketplace"></param>
    /// <returns></returns>
    /// <exception cref="Exception">Unable to identify 'core' type.</exception>
    public static ICore? GetCore(Marketplaces marketplace)
    {
      switch (marketplace)
      {
        default:
        case Marketplaces.NONE: throw new Exception(string.Format("Unable to identify type, please specify a type which implements type '0'.", nameof(ICore)));
        case Marketplaces.BINANCE: return new BinanceCore(LoggerFactory.CreateLogger<ICore>());
        case Marketplaces.COINBASE_PRO: return new BinanceCore(LoggerFactory.CreateLogger<ICore>());
      }
    }
  }
}
