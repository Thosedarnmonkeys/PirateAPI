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
    #region private fields
    private ContextMenu trayMenu;
    private NotifyIcon trayIcon;
    private PirateAPIHost apiHost;
    #endregion

    #region constructor
    public SystemTrayForm()
    {
      apiHost = PirateAPIHostBuilder.Build();

      if (apiHost.LoggingMode == LoggingMode.ConsoleWindow || apiHost.LoggingMode == LoggingMode.FileAndConsoleWindow)
      {
        AllocConsole();
        Console.Title = "PirateAPI Log";
      }
      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Current Proxy:");
      trayMenu.MenuItems.Add(apiHost?.BestProxy?.Domain);
      trayMenu.MenuItems.Add("Exit", OnExitClick);

      trayIcon = new NotifyIcon();
      trayIcon.Icon = new Icon(@"C:\Users\sennever.APTECO\Source\Repos\SurfacePenOnlyMode\SurfacePenOnlyMode\Images\PenIcon.ico");
      trayIcon.Text = "PirateAPI";
      trayIcon.ContextMenu = trayMenu;
      trayIcon.Visible = true;

      apiHost.ProxyUpdated += OnApiHostProxyUpdated;

      apiHost.StartServing();
    }
    #endregion

    #region private methods
    private void OnApiHostProxyUpdated(object sender, ProxyUpdatedEventArgs e)
    {
      trayMenu.MenuItems[1].Text = e.Proxy.Domain;
    }

    private void OnExitClick(object sender, EventArgs e)
    {
      Close();
    }
    #endregion

    #region extern methods
    [DllImport("kernel32.dll")]
    private static extern bool AllocConsole();
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
