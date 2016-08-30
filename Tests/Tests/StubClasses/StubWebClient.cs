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

    public StubWebClient(Func<string> responseStringFunc)
    {
      this.responseStringFunc = responseStringFunc;
    }

    public string DownloadString(string address)
    {
      return responseStringFunc.Invoke();
    }
  }
}
