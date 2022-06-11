using FractionalCryptoBot.Cores;
using Microsoft.Extensions.Logging;

namespace FractionalCryptoBot
{
  using FractionalCryptoBot.Enumerations;
  public class ApplicationEntry
  {
    static void Main(string[] args)
    {
      // Bootup the main core and instantiate a logger for it.
      MainCore mainCore = new(new LoggerFactory().CreateLogger<MainCore>());

      // Toggle the line below to run the program.
      // mainCore.RunMainProcedure();
      Console.ReadLine();
    }
  }
}
