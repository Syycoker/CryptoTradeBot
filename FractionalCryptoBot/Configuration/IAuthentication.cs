using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Configuration
{
  public interface IAuthentication
  {
    public bool SandboxMode { get; set; }
    public string Uri { get; }
    public string WebsocketUri { get; }
    public string Key { get; }
    public string Secret { get; }
    public string Pass { get; }
  }
}
