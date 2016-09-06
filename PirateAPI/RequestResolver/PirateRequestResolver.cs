﻿using PirateAPI.Parsers.Torrents;
using PirateAPI.Parsers.Torznab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    private DateTime currentDate = DateTime.Now;
    #endregion

    #region constructor
    public PirateRequestResolver(ILogger logger, IWebClient webClient, DateTime? currentDate = null)
    {
      this.logger = logger;
      this.webClient = webClient;
      if (currentDate.HasValue)
        this.currentDate = currentDate.Value;
    }
    #endregion

    #region public methods
    public List<Torrent> Resolve(PirateRequest request)
    {
      //if (request == null)
      //{
      //  logger.LogError("PirateRequestResolver.Resolve was passed a null request");
      //  return null;
      //}

      //string pirateProxy = string.IsNullOrWhiteSpace(request.PirateProxyURL) ? "" : request.PirateProxyURL;
      //string searchArg = string.IsNullOrWhiteSpace(request.ShowName) ? "" : request.ShowName.Replace("+", "%20");
      //double page = Math.Floor((double)request.Offset / 30);
      //string categories;
      //switch (request.Quality)
      //{
      //  case VideoQuality.SD:
      //    categories = "205";
      //    break;

      //  case VideoQuality.HD:
      //    categories = "208";
      //    break;

      //  case VideoQuality.Both:
      //    categories = "205,208";
      //    break;

      //  default:
      //    categories = "205";
      //    break;
      //}

      //List<string> rows = new List<string>();

      //while (rows.Count < request.Limit)
      //{
      //  string requestUrl = $"{pirateProxy}/search/{searchArg}/{page}/99/{categories}";
      //  string[] rowArray = GetRowsFromAddress(requestUrl);
      //  page++;
      //}

      throw new NotImplementedException();
    }
    #endregion
  }
}
