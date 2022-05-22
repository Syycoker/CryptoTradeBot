using FractionalCryptoBot.Cores;

namespace FractionalCryptoBot.Configuration
{
  /// <summary>
  /// Static class to define the settings the application will run under.
  /// </summary>
  public class ServiceSettings
  {
    #region Members
    /// <summary>
    /// Is the application going to run on the service's sandbox api endpoints?
    /// </summary>
    public bool IsSandboxMode { get; private set; } = false;

    /// <summary>
    /// Minimum amount of threads running to check the marketplace for a particular service.
    /// </summary>
    public int MinConcurrentThreads { get; private set; } = 100;

    /// <summary>
    /// The maximum amount of threads running to check the marketplace for a particular service.
    /// </summary>
    public int MaxConcurrentThreads { get; private set; } = 100;

    /// <summary>
    /// Determines whether the core whould be running or not.
    /// </summary>
    public bool ShouldRun { get; private set; } = true;
    #endregion
    #region Constructor
    public ServiceSettings(ICore core)
    {

    }
    #endregion
    #region Public
    /// <summary>
    /// Sets where the service's endpoints will go to, i.e. live marketplace or sandbox.
    /// </summary>
    /// <param name="enabled">Whether to make the application use sandbox settings or not.</param>
    public void SetSandboxMode(bool enabled)
    {
      IsSandboxMode = enabled;
    }

    /// <summary>
    /// Sets the minimum amount of threads running concurrently for all the services.
    /// </summary>
    /// <param name="minThreads"></param>
    public void SetMinConcurrentThreads(int minThreads)
    {
      MinConcurrentThreads = minThreads;
    }

    /// <summary>
    /// Sets the maximum amount of threads running concurrently for all the services.
    /// </summary>
    /// <param name="maxThreads"></param>
    public void SetMaxConcurrentThreads(int maxThreads)
    {
      MaxConcurrentThreads = maxThreads;
    }

    /// <summary>
    /// Public method to allow the user to determine whether the core should be running or not.
    /// </summary>
    /// <param name="value"></param>
    public void RunCore(bool value)
    {
      ShouldRun = value;
    }
    #endregion
  }
}
