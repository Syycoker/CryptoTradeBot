using FractionalCryptoBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Configuration
{
  public class Application : IApplication
  {
    IHttpService BinanceService;
    IHttpService CoinbaseService;

    public Application(IHttpService binanceService, IHttpService coinbaseService)
    {
      // Set up the Binance Service and any other service here.
      BinanceService = binanceService;
      CoinbaseService = coinbaseService;
    }

    public void Run()
    {

    }
  }
}
