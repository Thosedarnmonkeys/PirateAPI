using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.SanityCheckers
{
  public class TorrentNameSanityChecker
  {
    public bool Check(string showName, string input)
    {
      return CheckImpl(showName, null, null, input);
    }

    public bool Check(string showName, int season, string input)
    {
      return CheckImpl(showName, season, null, input);
    }

    public bool Check(string showName, int season, int episode, string input)
    {
      return CheckImpl(showName, season, season, input);
    }


    private bool CheckImpl(string showName, int? season, int? episode, string input)
    {

    }


  }
}
