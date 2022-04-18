using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;
using Timer = System.Windows.Forms.Timer;

namespace FractionalCryptoBot.Exceptions
{
  /// <summary>
  /// Class to sync the internal time of the computer to be within the leeway of calling any service's API Endpoint.
  /// Code was used from: https://www.codeproject.com/Articles/18918/Windows-System-Time-Synchronizer, many thanks.
  /// </summary>
  public class OutOfSyncException : Exception
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
      public short wYear;
      public short wMonth;
      public short wDayOfWeek;
      public short wDay;
      public short wHour;
      public short wMinute;
      public short wSecond;
      public short wMilliseconds;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetSystemTime([In] ref SYSTEMTIME st);

    public OutOfSyncException()
    {
      BooleanSwitch Tracer = new BooleanSwitch("TraceSwitch",
                "Trace for entire application");
      EventLog Log = null;

      try
      {
        // Initialize EventLog
        Log = new EventLog();
        if (!System.Diagnostics.EventLog.SourceExists("WinTimeSync"))
          System.Diagnostics.EventLog.CreateEventSource(
              "WinTimeSync", "Zion");
        Log.Source = "WinTimeSync";
        Log.Log = "Zion";

        if (Tracer.Enabled)
          Trace.Listeners.Add(new EventLogTraceListener(Log));

        Trace.WriteLineIf(Tracer.Enabled, "Starting WinTimeSync...");

        bool ServiceAlive = true;
        double CurrentTimestamp = 0;

        Trace.WriteLineIf(Tracer.Enabled,
            "Opening configuration file for initializing" +
            " global settings...");

        int SyncInterval = Convert.ToInt32(
            ConfigurationSettings.AppSettings[
            "SyncInterval"].ToString());
        string RequestURL = ConfigurationSettings.AppSettings[
            "TimeServerURL"].ToString();

        Trace.WriteLineIf(Tracer.Enabled,
            "Global settings initialized [SyncInterval: " +
            SyncInterval + " minutes; RequestURL: " +
            RequestURL + "]");

        while (ServiceAlive)
        {
          Trace.WriteLineIf(!Tracer.Enabled,
          "Synchronizing system time with time server...");

          // Send HTTP request for Timestamp
          Trace.WriteLineIf(Tracer.Enabled,
              "Creating HTTP request to [" + RequestURL + "]");
          WebRequest Req = WebRequest.Create(RequestURL);
          Trace.WriteLineIf(Tracer.Enabled,
              "Setting default proxy for the HTTP request...");
          Req.Proxy = WebProxy.GetDefaultProxy();
          Trace.WriteLineIf(Tracer.Enabled,
              "Sending HTTP request...");
          WebResponse Res = Req.GetResponse();

          // Save as Timestamp as a temporary XML file
          string TempFile = Guid.NewGuid().ToString() + ".xml";
          Trace.WriteLineIf(Tracer.Enabled,
              "Creating a temporary XML file [" + TempFile + "]");
          StreamWriter SW = new StreamWriter(TempFile);
          Trace.WriteLineIf(Tracer.Enabled,
              "Saving HTTP response to the temporary XML file...");
          SW.Write(new StreamReader(
              Res.GetResponseStream()).ReadToEnd());
          SW.Close();

          // Read the XML file and get the Timestamp value
          Trace.WriteLineIf(Tracer.Enabled,
              "Opening the temporary XML file...");
          XmlTextReader MyXML = new XmlTextReader(TempFile);
          Trace.WriteLineIf(Tracer.Enabled,
              "Reading the temporary XML file...");
          while (MyXML.Read())
          {
            switch (MyXML.NodeType)
            {
              case XmlNodeType.Element:
                if (MyXML.Name == "Timestamp")
                {

                  CurrentTimestamp = Convert.ToDouble(
                      MyXML.ReadInnerXml());
                }
                break;
            }
          }
          Trace.WriteLineIf(Tracer.Enabled,
              "Closing the temporary XML file...");
          MyXML.Close();

          // Delete the temporary XML file
          FileInfo TFile = new FileInfo(TempFile);
          Trace.WriteLineIf(Tracer.Enabled,
              "Deleting the temporary XML file...");
          TFile.Delete();

          // Convert Timestamp to Time
          DateTime MyDateTime =
              new DateTime(1970, 1, 1, 0, 0, 0, 0);

          MyDateTime = MyDateTime.AddSeconds(CurrentTimestamp);


                    // Change the system time
          SYSTEMTIME SysTime = new SYSTEMTIME();
          SysTime.wYear = (short)MyDateTime.Year;
          SysTime.wMonth = (short)MyDateTime.Month;
          SysTime.wDay = (short)MyDateTime.Day;
          SysTime.wHour = (short)MyDateTime.Hour;
          SysTime.wMinute = (short)MyDateTime.Minute;
          SysTime.wSecond = (short)MyDateTime.Second;
          Trace.WriteLineIf(Tracer.Enabled,
              "Setting the system time...");
          SetSystemTime(ref SysTime);

          Trace.WriteLineIf(Tracer.Enabled,
              "Switching to sleep state until SyncInterval...");
          for (int i = 0; i < SyncInterval; i++)
            Thread.Sleep(1000 * 60);
          Trace.WriteLineIf(Tracer.Enabled,
              "Switching back to active mode...");
        }
      }
      catch (Exception Ex)
      {
        // Log for errors                
        if (Tracer.Enabled)
          Log.WriteEntry(Ex.StackTrace, EventLogEntryType.Error);
        else
          Log.WriteEntry(Ex.Message, EventLogEntryType.Error);
      }
    }
  }
}
