using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.WebClient;

namespace PirateAPITests.Tests.StubClasses
{
  public class StubWebClient : IWebClient
  {
    private List<string> responseStrings;

    public List<string> RequestsMade { get; set; } = new List<string>();

    public StubWebClient(List<string> responseStrings)
    {
      this.responseStrings = responseStrings;
    }

    public string DownloadString(string address)
    {
      RequestsMade.Add(address);

      string response = responseStrings.First();

      responseStrings.RemoveAt(0);

      return response;
    }
  }
}
