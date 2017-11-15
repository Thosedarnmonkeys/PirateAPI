using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PirateAPI.WebClient;

namespace PirateAPITests.Tests.StubClasses
{
  public class StubWebClient : IWebClient
  {
    private List<string> responseStrings;
    public Dictionary<int, int> pageDelays;

    public List<string> RequestsMade { get; set; } = new List<string>();

    public int TimeoutMillis => 0;

    public StubWebClient(List<string> responseStrings)
    {
      this.responseStrings = responseStrings;
    }

    public string DownloadString(string address)
    {
      if (pageDelays != null && pageDelays.ContainsKey(RequestsMade.Count))
        Thread.Sleep(pageDelays[RequestsMade.Count]);

      RequestsMade.Add(address);

      string response = responseStrings.First();

      responseStrings.RemoveAt(0);

      return response;
    }
  }
}
