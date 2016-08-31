using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.WebServer
{
  public class BasicWebServer : IWebServer
  {
    #region private fields
    private HttpListener listener;
    private Func<string, string> responseFunc;
    private ILogger logger;

    private int port;
    private string webroot;
    #endregion

    #region constructor
    public BasicWebServer(string webroot, int port, Func<string, string> responseFunc, ILogger logger)
    {
      this.webroot = webroot;
      this.port = port;
      this.responseFunc = responseFunc;
      this.logger = logger;
    }
    #endregion

    #region public methods
    public bool StartServing()
    {
      try
      {
        if (listener != null)
        {
          logger.LogError("WebServer.StartServing was called when listener is not null");
          return false;
        }

        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}{webroot}/");
        listener.Start();
        listener.BeginGetContext(ServeRequest, listener);
        logger.LogMessage($"WebServer started serving on address {listener.Prefixes.First()}");
        return true;
      }
      catch (Exception e)
      {
        logger.LogException(e, "WebServer.StartServing threw exception: ");
        return false;
      }
    }

    public void StopServing()
    {
      try
      {
        listener.Stop();
        listener.Close();
        listener = null;
        logger.LogMessage("WebServer has stopped serving");
      }
      catch (Exception e)
      {
        logger.LogException(e, "WebServer.StopServing threw exception: ");
        throw;
      }
    }

    #endregion

    #region private methods
    private void ServeRequest(IAsyncResult asyncResult)
    {
      HttpListener asyncListener = asyncResult.AsyncState as HttpListener;
      if (asyncListener == null)
        throw new ArgumentException("Async method WebServer.ServerRequest was passed an object of type" + (asyncResult.AsyncState == null ? "null" : asyncResult.AsyncState.ToString()) + " instead of an HttpListener");

      HttpListenerContext context = asyncListener.EndGetContext(asyncResult);
      asyncListener.BeginGetContext(ServeRequest, asyncListener);

      string response = responseFunc.Invoke(context.Request.RawUrl);

      byte[] buffer = Encoding.UTF8.GetBytes(response);
      context.Response.ContentLength64 = buffer.Length;
      context.Response.OutputStream.Write(buffer, 0, buffer.Length);
      context.Response.OutputStream.Close();
    }
    #endregion
  }
}
