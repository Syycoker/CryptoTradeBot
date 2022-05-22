using FractionalCryptoBot.Cores;
using Microsoft.Extensions.Logging;

namespace FractionalCryptoBot
{
  public class ApplicationEntry
  {
    static void Main(string[] args)
    {
      // Bootup the main core and instantiate a logger for it.
      MainCore mainCore = new(new LoggerFactory().CreateLogger<MainCore>());

      Console.ReadLine();
    }
  }
}
