using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Models
{
  /// <summary>
  /// DTO class to represent currencies that are common in however many other services.
  /// </summary>
  public class SharedCrypto
  {
    #region Members
    /// <summary>
    /// Immutable collection of crypto.
    /// </summary>
    public readonly IEnumerable<Crypto> CommonCurrencies;
    #endregion
    #region Constructor
    /// <summary>
    /// Public constructor that takes in a variable number of Crypto DTOs.
    /// </summary>
    /// <param name="cryptos"></param>
    public SharedCrypto(params Crypto[] cryptos)
    {
      CommonCurrencies = cryptos;
    }
    #endregion
  }
}
