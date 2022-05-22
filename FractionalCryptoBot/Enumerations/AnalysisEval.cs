using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Enumerations
{
  /// <summary>
  /// Enumeration to determiine how well an asset is doing in its current state.
  /// </summary>
  public enum AnalysisEval
  {
    NONE = 0,
    ABYSMAL = 1,
    VERY_POOR = 2,
    POOR = 3,
    SATISFACTORY = 4,
    GOOD = 5,
    GREAT = 6,
    EXCELLENT = 7,
  }
}
