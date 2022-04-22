using Autofac;
using FractionalCryptoBot.Services;

namespace FractionalCryptoBot.Configuration
{
  public static class ContainerConfig
  {
    public static IContainer Configure()
    {
      var builder = new ContainerBuilder();

      builder.RegisterType<Application>().As<IApplication>();
      // builder.RegisterType<BinanceService>().As<IHttpService>();

      return builder.Build();
    }
  }
}
