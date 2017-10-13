using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.RequestResolver;
using PirateAPI.WebClient;

namespace PirateAPI
{
  public static class PirateAPIHostBuilder
  {
    #region PirateAPIHost default config values
    private const string defaultWebRoot = "";
    private const int defaultPort = 8080;
    private static readonly ReadOnlyCollection<string> defaultLocationPrefs = new ReadOnlyCollection<string>(new List<string>() { "uk" });
    private static readonly ReadOnlyCollection<string> defaultBlackList = new ReadOnlyCollection<string>(new List<string>());
    private static readonly TimeSpan defaultProxyRefreshInterval = new TimeSpan(1, 0, 0);
    private const bool defaultMagnetSearchProxiesOnly = false;
    private const int defaultAPILimit = 30;
    private static readonly string defaultLogFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "PirateAPILog.txt";
    private static readonly ILogger defaultLogger = new FileAndConsoleLogger(defaultLogFilePath);
    private static readonly PirateRequestResolveStrategy defaultStrategy = PirateRequestResolveStrategy.Parallel;
    private const int defaultWebClientTimoutMillis = 10000;
    #endregion

    #region ini file param names
    private const string webRootName = "webroot";
    private const string portName = "port";
    private const string locPrefName = "locationpreferences";
    private const string blacklistName = "blacklistedproxies";
    private const string refreshIntName = "currentproxyrefreshinterval";
    private const string magSearchOnlyName = "useproxieswithmagnetsinsearchonly";
    private const string apiLimitName = "maxsearchresults";
    private const string logPathName = "logfilepath";
    private const string loggingName = "loggingmode";
    private const string requestResolveMode = "requestresolvemode";
    private const string requestTimeoutMillis = "requesttimeoutmillis";
    #endregion

    #region private consts
    private const string iniFileName = "PirateAPI.ini";
    #endregion

    #region public methods
    public static PirateAPIHost Build()
    {
      Dictionary<string, string> config = ReadIniFileOptions();

      string webRoot = config.ContainsKey(webRootName) ? config[webRootName] : defaultWebRoot;

      int port;
      if (!(config.ContainsKey(portName) && int.TryParse(config[portName], out port)))
        port = defaultPort;

      List<string> locationPrefs = config.ContainsKey(locPrefName) ? TurnCommaSeparatedStringToList(config[locPrefName]) : defaultLocationPrefs.ToList();
      List<string> blackList = config.ContainsKey(blacklistName) ? TurnCommaSeparatedStringToList(config[blacklistName]) : defaultBlackList.ToList();
      TimeSpan proxyRefreshInterval = config.ContainsKey(refreshIntName) ? ParseTimeSpan(config[refreshIntName]) : defaultProxyRefreshInterval;

      bool magnetSearchProxiesOnly;
      if (!(config.ContainsKey(magSearchOnlyName) && bool.TryParse(config[magSearchOnlyName], out magnetSearchProxiesOnly)))
        magnetSearchProxiesOnly = defaultMagnetSearchProxiesOnly;

      int apiLimit;
      if (!(config.ContainsKey(apiLimitName) && int.TryParse(config[apiLimitName], out apiLimit)))
        apiLimit = defaultAPILimit;

      PirateRequestResolveStrategy resolveStrat = config.ContainsKey(requestResolveMode) ? ParseResolveStrategy(config[requestResolveMode]) : defaultStrategy;

      string logFilePath = config.ContainsKey(logPathName) ? config[logPathName] : defaultLogFilePath;
      if (string.IsNullOrWhiteSpace(logFilePath))
        logFilePath = defaultLogFilePath;

      ILogger logger = config.ContainsKey(loggingName) ? ParseLoggingMode(config[loggingName], logFilePath) : defaultLogger;

      int timeoutMillis;
      if (!(config.ContainsKey(requestTimeoutMillis) && int.TryParse(config[requestTimeoutMillis], out timeoutMillis)))
        timeoutMillis = defaultWebClientTimoutMillis;

      IWebClient webClient = new BasicWebClient(timeoutMillis, logger);

      PirateAPIHost host = new PirateAPIHost(webRoot, port, locationPrefs, blackList, proxyRefreshInterval, magnetSearchProxiesOnly, apiLimit, resolveStrat, logger, webClient);
      return host;
    }
    #endregion

    #region private methods
    private static Dictionary<string, string> ReadIniFileOptions()
    {
      string iniFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + iniFileName;

      if (!File.Exists(iniFilePath))
        CreateDefaultIniFile();

      string[] iniFileLines = File.ReadAllLines(iniFilePath);
      iniFileLines = (from l in iniFileLines
                      where !l.StartsWith("#") && !string.IsNullOrWhiteSpace(l)
                      select l).ToArray();

      Dictionary<string, string> valsDict = new Dictionary<string, string>();

      foreach (string line in iniFileLines)
      {
        string[] splitLine = line.Split('=');
        if (splitLine.Length != 2)
          continue;

        valsDict[splitLine[0]] = splitLine[1];
      }

      return valsDict;
    }

    private static void CreateDefaultIniFile()
    {
      string iniFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + iniFileName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (Stream stream = assembly.GetManifestResourceStream("PirateAPI.PirateAPI.ini"))
      {
        if (stream == null)
          throw new Exception("Assembly.GetManifestResourceStream return a null stream for name PirateAPIConsole.PirateAPIConsole.ini");

        using (StreamReader reader = new StreamReader(stream))
        {
          string defaultIniFileContents = reader.ReadToEnd();
          File.WriteAllText(iniFilePath, defaultIniFileContents);
        }
      }
    }

    private static List<string> TurnCommaSeparatedStringToList(string input)
    {
      List<string> list = new List<string>();

      if (!input.Contains(","))
      {
        list.Add(input);
        return list;
      }

      string[] vals = input.Split(',');
      list.AddRange(vals);
      return list;
    }

    private static TimeSpan ParseTimeSpan(string input)
    {
      List<string> list = TurnCommaSeparatedStringToList(input);

      if (list.Count != 4)
        return defaultProxyRefreshInterval;

      int days;
      if (!int.TryParse(list[0], out days))
        return defaultProxyRefreshInterval;

      int hours;
      if (!int.TryParse(list[1], out hours))
        return defaultProxyRefreshInterval;

      int mins;
      if (!int.TryParse(list[2], out mins))
        return defaultProxyRefreshInterval;

      int secs;
      if (!int.TryParse(list[3], out secs))
        return defaultProxyRefreshInterval;

      return new TimeSpan(days, hours, mins, secs);
    }

    private static ILogger ParseLoggingMode(string input, string logFilePath)
    {
      LoggingMode mode;
      if (!Enum.TryParse(input, out mode))
        return defaultLogger;

      switch (mode)
      {
        case LoggingMode.ConsoleWindow:
          return new ConsoleLogger();

        case LoggingMode.File:
          return new FileLogger(logFilePath);

        case LoggingMode.FileAndConsoleWindow:
          return new FileAndConsoleLogger(logFilePath);

        default:
          return defaultLogger;
      }
    }

    private static PirateRequestResolveStrategy ParseResolveStrategy(string input)
    {
      PirateRequestResolveStrategy strategy;

      if (!Enum.TryParse(input, out strategy))
        return defaultStrategy;

      return strategy;
    }
    #endregion
  }
}
