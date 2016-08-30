using PirateAPI.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.WebClient;

namespace PirateAPI.RequestResolver
{
  public class PirateRequestResolver
  {
    #region private fields
    private ILogger logger;
    private IWebClient webClient;
    #endregion

    #region constructor
    public PirateRequestResolver(ILogger logger, IWebClient webClient)
    {
      this.logger = logger;
      this.webClient = webClient;
    }
    #endregion

    #region public methods
    public string Resolve(PirateRequest request)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
