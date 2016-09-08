using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.Parsers.Torrents;

namespace PirateAPI.ResponseBuilders
{
  public class TorznabResponseBuilder
  {
    #region private fields
    private ILogger logger;
    #endregion

    #region constructor
    public TorznabResponseBuilder(ILogger logger)
    {
      this.logger = logger;
    } 
    #endregion

    #region public methods
    public string BuildResponse(List<Torrent> torrents)
    {
      throw new NotImplementedException();
    } 
    #endregion
  }
}
