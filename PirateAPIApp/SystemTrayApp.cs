using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PirateAPI;
using PirateAPI.EventArgTypes;
using PirateAPI.Logging;

namespace PirateAPIApp
{
  public class SystemTrayForm : Form
  {
    #region private delegate types
    private delegate void ConsoleEventDelegate(int eventType);
    #endregion

    #region private fields

    private ContextMenuStrip trayMenu;
    private NotifyIcon trayIcon;
    private PirateAPIHost apiHost;
    private ConsoleEventDelegate consoleHandler;
    #endregion

    #region constructor

    public SystemTrayForm()
    {
      apiHost = PirateAPIHostBuilder.Build();

      if (apiHost.LoggingMode == LoggingMode.ConsoleWindow || apiHost.LoggingMode == LoggingMode.FileAndConsoleWindow)
      {
        AllocConsole();
        Console.Title = "PirateAPI Log";
        consoleHandler = HandleConsoleClose;
        SetConsoleCtrlHandler(consoleHandler, true);
      }
      trayMenu = new ContextMenuStrip();
      trayMenu.Items.Add(new ToolStripLabel("Current Proxy:"));
      trayMenu.Items.Add(new ToolStripLabel(""));
      trayMenu.Items.Add("-");
      trayMenu.Items.Add("Refresh proxies now", null, OnClickRefreshNow);
      trayMenu.Items.Add("-");
      trayMenu.Items.Add("Exit", null, OnExitClick);

      trayIcon = new NotifyIcon();
      trayIcon.Icon =
        new Icon(@"C:\Users\sennever.APTECO\Source\Repos\SurfacePenOnlyMode\SurfacePenOnlyMode\Images\PenIcon.ico");
      trayIcon.Text = "PirateAPI";
      trayIcon.ContextMenuStrip = trayMenu;
      trayIcon.Visible = true;

      apiHost.ProxyUpdated += OnApiHostProxyUpdated;

      apiHost.StartServing();
    }

    #endregion

    #region private methods

    private void OnApiHostProxyUpdated(object sender, ProxyUpdatedEventArgs e)
    {
      trayMenu.Items[1].Text = e.Proxy.Domain;
    }

    private void OnClickRefreshNow(object sender, EventArgs e)
    {
      apiHost?.RefreshProxies();
    }

    private void OnExitClick(object sender, EventArgs e)
    {
      Close();
    }

    private void HandleConsoleClose(int eventType)
    {
      if (eventType == 2)
      {
        trayIcon.Dispose();
        apiHost.ProxyUpdated -= OnApiHostProxyUpdated;
      }
    }
    #endregion

    #region extern methods
    [DllImport("kernel32.dll")]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    #endregion

    #region overriden methods
    protected override void OnLoad(EventArgs e)
    {
      Visible = false;
      ShowInTaskbar = false;

      base.OnLoad(e);
    }

    protected override void Dispose(bool disposing)
    {
      trayIcon.Dispose();
      apiHost.ProxyUpdated -= OnApiHostProxyUpdated;

      base.Dispose(disposing);
    }
    #endregion
  }
}
