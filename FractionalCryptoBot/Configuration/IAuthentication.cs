using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Configuration
{
  public interface IAuthentication
  {
    public string Exchange { get; }
    public string ApiKey { get; }
    public string ApiSecret { get; }
    public string ApiPass { get; }
  }
}
