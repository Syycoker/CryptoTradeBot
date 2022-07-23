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
      FTXCore _ftxCore = new(LoggerFactory.CreateLogger<ICore>());
      // Return the collection of services where their exhcnage's are currently active...
      return new List<ICore>() 
      { _binanceCore, _coinbaseProCore, _ftxCore }
      .Where(core => core.ActiveService().Result);
    }

    /// <summary>
    /// Get a specific core based on the marketplaces enum.
    /// </summary>
    /// <param name="marketplace"></param>
    /// <returns></returns>
    /// <exception cref="Exception">Unable to identify 'core' type.</exception>
    public static ICore? GetCore(Marketplaces marketplace)
    {
      var logger = LoggerFactory.CreateLogger<ICore>();

      switch (marketplace)
      {
        default:
        case Marketplaces.NONE: throw new Exception(string.Format("Unable to identify type, please specify a type which implements type '0'.", nameof(ICore)));
        case Marketplaces.BINANCE: return new BinanceCore(logger);
        case Marketplaces.COINBASE_PRO: return new CBProCore(logger);
        case Marketplaces.FTX: return new FTXCore(logger);
      }
    }
  }
}
