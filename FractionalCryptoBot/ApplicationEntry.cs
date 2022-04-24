using Autofac;
using FractionalCryptoBot.Configuration;

namespace FractionalCryptoBot
{
  public class ApplicationEntry
  {
    static void Main(string[] args)
    {
      var cores = CoreFactory.GetCores();

      if (cores is null || !cores.Any())
        return;

      // Run all the cores
      cores.ToList().ForEach(core => core.Run());

      Console.ReadLine();
    }
  }
}
