using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.WebClient;

namespace Tests.Tests.StubClasses
{
  public class StubWebClient : IWebClient
  {
    private Func<string> responseStringFunc;

    public string LastRequest { get; set; }

    public StubWebClient(Func<string> responseStringFunc)
    {
      this.responseStringFunc = responseStringFunc;
    }

    public string DownloadString(string address)
    {
      LastRequest = address;
      return responseStringFunc.Invoke();
    }
  }
}
