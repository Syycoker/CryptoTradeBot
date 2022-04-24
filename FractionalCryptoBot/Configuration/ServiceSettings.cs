using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionalCryptoBot.Configuration
{
  /// <summary>
  /// Static class to define the settings the application will run under.
  /// </summary>
  public class ServiceSettings
  {
    /// <summary>
    /// Is the application going to run on the service's sandbox api endpoints?
    /// </summary>
    public static bool IsSandboxMode { get; private set; } = false;

    /// <summary>
    /// Minimum amount of threads running to check the marketplace for a particular service.
    /// </summary>
    public static int MinConcurrentThreads { get; private set; } = 100;

    /// <summary>
    /// The maximum amount of threads running to check the marketplace for a particular service.
    /// </summary>
    public static int MaxConcurrentThreads { get; private set; } = 100;


    /// <summary>
    /// Sets where the service's endpoints will go to, i.e. live marketplace or sandbox.
    /// </summary>
    /// <param name="enabled">Whether to make the application use sandbox settings or not.</param>
    public static void SetSandboxMode(bool enabled)
    {
      IsSandboxMode = enabled;
    }

    /// <summary>
    /// Sets the minimum amount of threads running concurrently for all the services.
    /// </summary>
    /// <param name="minThreads"></param>
    public static void SetMinConcurrentThreads(int minThreads)
    {
      MinConcurrentThreads = minThreads;
    }

    /// <summary>
    /// Sets the maximum amount of threads running concurrently for all the services.
    /// </summary>
    /// <param name="maxThreads"></param>
    public static void SetMaxConcurrentThreads(int maxThreads)
    {
      MaxConcurrentThreads = maxThreads;
    }
  }
}
