using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Enumerations
{
  public enum CoreStatus
  {
    NONE = 0,
    OUT_OF_SYNC = 1,
    INSUFFICIENT_FUNDS = 2,
    BUY_SUCCESSFUL = 3,
    BUY_UNSUCCESSFUL = 4,
    SELL_SUCCESSFUL = 5,
    SELL_UNSUCCESSFUL = 6,
    UNKNOWN_ERROR = 7,
  }
}
