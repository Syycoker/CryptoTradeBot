using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Configuration
{
  public class ExchangeAuth
  {
    public List<ExchangeAuthentication> Exchanges { get; set; } = new();
  }
}
