using PirateAPI.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyProviders.URLFile
{
  public class URLFileProvider : IProxyProvider
  {
    #region private consts
    private const string fileName = "PirateBayURLs.txt";
    #endregion

    #region private variables
    private ILogger logger;
    #endregion

    #region constructor
    public URLFileProvider(ILogger logger)
    {
      this.logger = logger;
    }
    #endregion

    #region public methods
    public List<Proxy> ListMagnetInSearchProxies()
    {
      string filePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + fileName;
      if (!File.Exists(filePath))
      {
        logger.LogError($"Couldn't find file at {filePath}");
        return new List<Proxy>();
      }

      var proxies = new List<Proxy>();

      using (FileStream stream = File.OpenRead(filePath))
      {
        using (StreamReader reader = new StreamReader(stream))
        {
          while (!reader.EndOfStream)
          {
            string line = reader.ReadLine();
            string[] splitLine = line.Split(',');

            if (splitLine.Length != 4)
            {
              logger.LogError($"Line {line} is not 4 comma separated values");
              continue;
            }

            bool hasMagnets;
            if (!bool.TryParse(splitLine[3], out hasMagnets))
            {
              logger.LogError($"Couldn't parse value {splitLine[3]} to bool");
              continue;
            }

            if (!hasMagnets)
              continue;

            ProxySpeed speed;
            if (!Enum.TryParse(splitLine[2], out speed))
            {
              logger.LogError($"Couldn't parse value {splitLine[2]} to ProxySpeed");
              speed = ProxySpeed.Slow;
            }

            var proxy = new Proxy()
            {
              Domain = splitLine[0],
              Country = splitLine[1],
              Speed = speed
            };

            proxies.Add(proxy);
          }
        }
      }

      return proxies;

    }

    public List<Proxy> ListProxies()
    {
      string filePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + fileName;
      if (!File.Exists(filePath))
      {
        logger.LogError($"Couldn't find file at {filePath}");
        return new List<Proxy>();
      }

      var proxies = new List<Proxy>();

      using (FileStream stream = File.OpenRead(filePath))
      {
        using (StreamReader reader = new StreamReader(stream))
        {
          while(!reader.EndOfStream)
          {
            string line = reader.ReadLine();
            string[] splitLine = line.Split(',');

            if (splitLine.Length != 4)
            {
              logger.LogError($"Line {line} is not 4 comma separated values");
              continue;
            }

            ProxySpeed speed;
            if (!Enum.TryParse(splitLine[2], out speed))
            {
              logger.LogError($"Couldn't parse value {splitLine[2]} to ProxySpeed");
              speed = ProxySpeed.Slow;
            }

            var proxy = new Proxy()
            {
              Domain = splitLine[0],
              Country = splitLine[1],
              Speed = speed
            };

            proxies.Add(proxy);
          }
        }
      }

      return proxies;
    }
    #endregion

  }
}
