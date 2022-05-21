using FractionalCryptoBot.Cores;
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

      // Return the collection of services where their exhcnage's are currently active...
      return new List<ICore>() { _binanceCore }.Where(core => core.ActiveService().Result);
    }
  }
}
