using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application = System.Windows.Application;
using static ARKServerCreationTool.ASCTTools;


namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for ServerList.xaml
    /// </summary>
    public partial class ServerList : Window
    {
        ASCTGlobalConfig config = ASCTGlobalConfig.Instance;

        public ServerList()
        {
            InitializeComponent();

            UpdateList();
        }

        public void UpdateList()
        {
            Dispatcher.Invoke(() =>
            {
                dg_ServerList.ItemsSource = config.Servers;

                dg_ServerList.Items.Refresh();

                UpdateButtons();
            });
        }

        private void btn_mainConfig_Click(object sender, RoutedEventArgs e)
        {
            ASCTTools.FindOrCreateWindow<ASCTConfigWindow>();
        }

        private void dg_ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtons();
        }

        RunButtonStatus runButtonStatus = RunButtonStatus.Unknown;

        private void UpdateButtons()
        {
            btn_run.IsEnabled = btn_deleteServer.IsEnabled = btn_viewServer.IsEnabled = dg_ServerList.SelectedItem != null;

            if (btn_run.IsEnabled)
            {
                if (!((ASCTServerConfig)dg_ServerList.SelectedItem).ProcessManager.IsRunning)
                {
                    btn_run.Content = "Start Server";
                    runButtonStatus = RunButtonStatus.Run;
                }
                else
                {
                    btn_run.Content = "Stop Server";
                    runButtonStatus = RunButtonStatus.Stop;
                }
            }
            else
            {
                btn_run.Content = "Select a server";
                runButtonStatus = RunButtonStatus.Unknown;
            }

            var serversRunning = config.Servers.Select(s => s.ProcessManager.IsRunning);

            btn_startAll.IsEnabled = serversRunning.Any(b => !b);
            btn_stopAll.IsEnabled = serversRunning.Any(b => b);
        }

        private void btn_viewServer_Click(object sender, RoutedEventArgs e)
        {
            ASCTTools.FindOrCreateServerWindow(((ASCTServerConfig)dg_ServerList.SelectedItem).ID);
        }

        private void btn_deleteServer_Click(object sender, RoutedEventArgs e)
        {
            var removeResult = System.Windows.MessageBox.Show("Are you sure you want to remove this server?", "Are you sure?", MessageBoxButton.YesNo);

            if (removeResult == MessageBoxResult.Yes)
            {
                var deleteResult = System.Windows.MessageBox.Show("Would you like to permenantly delete the files for the server?", "Delete Files?", MessageBoxButton.YesNoCancel);

                if (deleteResult == MessageBoxResult.Cancel)
                {
                    return; //Do nothing, user canceled on the second prompt
                }

                string path = ((ASCTServerConfig)dg_ServerList.SelectedItem).GameDirectory;

                config.Servers.Remove(((ASCTServerConfig)dg_ServerList.SelectedItem)).ToString();
                System.Windows.MessageBox.Show(config.Servers.Count.ToString());

                config.Save();

                if (deleteResult == MessageBoxResult.Yes)
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }

                UpdateList();
            }
        }

        private void btn_addServer_Click(object sender, RoutedEventArgs e)
        {
            ASCTTools.FindOrCreateServerConfigWindow(null);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void btn_RunServer_Click(object sender, RoutedEventArgs e)
        {
            if (runButtonStatus == RunButtonStatus.Unknown)
            {
                UpdateButtons(); 
                return;
            }

            ASCTServerConfig selectedServer = ((ASCTServerConfig)dg_ServerList.SelectedItem);
            bool selectedRunning = selectedServer.ProcessManager.IsRunning;

            var cluster = config.Servers.AsParallel().Where(s => s.ClusterKey == selectedServer.ClusterKey && selectedRunning == s.ProcessManager.IsRunning);

            bool launchOne = true; //Set to false if we act on the cluster

            if (selectedServer.ClusterKey != string.Empty && cluster.Count() > 1)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Would you like to perform this action on all of the servers in the same cluster?", "", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    ASCTServerConfig[] serversInCluster = cluster.ToArray();

                    Parallel.For(0, serversInCluster.Length, i =>
                    {
                        if (runButtonStatus == RunButtonStatus.Run)
                        {
                            serversInCluster[i].ProcessManager.Start();
                        }
                        else if (runButtonStatus == RunButtonStatus.Stop)
                        {
                            serversInCluster[i].ProcessManager.Stop();
                        }
                    });

                    launchOne = false;
                }
            }
            
            if (launchOne)
            {
                if (runButtonStatus == RunButtonStatus.Run)
                {
                    selectedServer.ProcessManager.Start();
                }
                else if (runButtonStatus == RunButtonStatus.Stop)
                {
                    selectedServer.ProcessManager.Stop();
                }
            }

            UpdateList();
        }

        private void btn_startAll_Click(object sender, RoutedEventArgs e)
        {
            Parallel.For(0, config.Servers.Count, i =>
            {
                config.Servers[i].ProcessManager.Start();
            });
            UpdateList();

        }

        private void btn_stopAll_Click(object sender, RoutedEventArgs e)
        {
            Parallel.For(0, config.Servers.Count, i =>
            {
                config.Servers[i].ProcessManager.Stop();
            });
            UpdateList();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Application.Current.Windows.Count > 1)
            {
                System.Windows.Forms.MessageBox.Show("This window can not be closed while others are still open");
                e.Cancel = true;
            }
        }
    }
}



