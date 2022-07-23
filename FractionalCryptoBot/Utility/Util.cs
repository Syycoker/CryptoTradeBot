using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Utility
{
  public static class Util
  {
    private static DateTime _epochTime = new DateTime(1970, 1, 1, 0, 0, 0);

    public static long GetMillisecondsFromEpochStart()
    {
      return GetMillisecondsFromEpochStart(DateTime.UtcNow);
    }
    public static long GetMillisecondsFromEpochStart(DateTime time)
    {
      return (long)(time - _epochTime).TotalMilliseconds;
    }

    public static long GetSecondsFromEpochStart(DateTime time)
    {
      return (long)(time - _epochTime).TotalSeconds;
    }
  }
}
