using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ARKServerCreationTool
{
    public static class ASCTTools
    {
        public static void FindOrCreateWindow<T>() where T : Window, new()
        {
            T desiredWindow = null;

            desiredWindow = Application.Current.Windows.OfType<T>().FirstOrDefault();

            if (desiredWindow != null)
            {
                desiredWindow.Activate();
            }
            else
            {
                desiredWindow = new T();
                desiredWindow.Show();
            }
        }

        public static void FindAndUpdateListWindow()
        {
            var windows = Application.Current.Windows.OfType<ServerList>().ToArray();

            for (int i = 0; i < windows.Length; i++)
            {
                windows[i].UpdateList();
            }
        }

        public static void FindOrCreateServerWindow(int serverID)
        {
            ServerWindow desiredWindow = null;

            desiredWindow = Application.Current.Windows.OfType<ServerWindow>().Where(w => w.targetServerID == serverID).FirstOrDefault();

            if (desiredWindow != null)
            {
                desiredWindow.Activate();
            }
            else
            {
                desiredWindow = new ServerWindow(serverID);
                desiredWindow.Show();
            }
        }

        public static void FindOrCreateServerConfigWindow(int? serverID)
        {
            ServerConfigurationWindow desiredWindow = null;

            desiredWindow = Application.Current.Windows.OfType<ServerConfigurationWindow>().Where(w => w.targetServerID == serverID).FirstOrDefault();

            if (desiredWindow != null)
            {
                desiredWindow.Activate();
            }
            else
            {
                desiredWindow = new ServerConfigurationWindow(serverID);
                desiredWindow.Show();
            }
        }

        public static void FindOrCreateServerUpdaterWindow(int serverID)
        {
            UpdaterWindow desiredWindow = null;

            desiredWindow = Application.Current.Windows.OfType<UpdaterWindow>().FirstOrDefault();

            if (desiredWindow != null)
            {
                desiredWindow.Activate();
            }
            else
            {
                desiredWindow = new UpdaterWindow(new HashSet<int> { serverID });
                desiredWindow.Show();
            }
        }

        public enum RunButtonStatus
        {
            Unknown, Run, Stop
        }
    }
}
