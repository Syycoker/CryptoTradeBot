using Autofac;
using FractionalCryptoBot.Configuration;

namespace FractionalCryptoBot
{
  public class ApplicationEntry
  {
    static void Main(string[] args)
    {
      var container = ContainerConfig.Configure();

      using (var scope = container.BeginLifetimeScope())
      {
        var app = scope.Resolve<IApplication>();
        app.Run();
      }

      Console.ReadLine();
    }
  }
}
