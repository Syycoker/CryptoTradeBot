using FractionalCryptoBot.Cores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public static IEnumerable<ICore>? GetCores()
    {
      BinanceCore _binanceCore = new(LoggerFactory.CreateLogger<ICore>());
      return new List<ICore>() { _binanceCore };
    }
  }
}
