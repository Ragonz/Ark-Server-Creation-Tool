using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
//using System.Windows.Forms;
using System.IO.Compression;
using System.Diagnostics;
using System.Printing;
using System.Text.RegularExpressions;
using MessageBox = System.Windows.MessageBox;
using System.Collections.Concurrent;

namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UpdaterWindow : Window
    {
        private ASCTGlobalConfig config => ASCTGlobalConfig.Instance;

        List<UpdatableServerListEntry> updatableServers = new List<UpdatableServerListEntry>();

        public UpdaterWindow(HashSet<int> preselectServerIDs = null)
        {
            InitializeComponent();

            UpdateUI(preselectServerIDs);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI(HashSet<int> preselectServerIDs = null)
        {
            RefreshUpdateableServerList(preselectServerIDs);

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btn_runUpdate.IsEnabled = !IsUpdating && dg_updatableServers.SelectedItems.Count > 0;
            txt_updateConsole.Text = updateLog;
            chk_validate.IsChecked = config.validateUpdates;
        }

        private static bool IsUpdating => updateTask != null && updateTask.Status is TaskStatus.Running or TaskStatus.WaitingToRun;

        private void RefreshUpdateableServerList(HashSet<int> preselectServerIDs = null)
        {
            if (dg_updatableServers.SelectedItems != null)
            {
                if (preselectServerIDs == null)
                {
                    preselectServerIDs = new HashSet<int>();
                }
                for (int i = 0; i < dg_updatableServers.SelectedItems.Count; i++)
                {
                    preselectServerIDs.Add(((UpdatableServerListEntry)dg_updatableServers.SelectedItems[i]).targetServerID);
                }
            }

            updatableServers.Clear();

            foreach (ASCTServerConfig item in config.Servers)
            {
                updatableServers.Add(new UpdatableServerListEntry(item.ID));
            }

            dg_updatableServers.ItemsSource = updatableServers;

            if (preselectServerIDs != null)
            {

                for (int i = 0; i < dg_updatableServers.Items.Count; i++)
                {
                    if (preselectServerIDs.Contains(((UpdatableServerListEntry)dg_updatableServers.Items[i]).targetServerID))
                    {
                        dg_updatableServers.SelectedItems.Add(dg_updatableServers.Items[i]);
                    }
                }
            }
            dg_updatableServers.Items.Refresh();

            if (dg_updatableServers.IsEnabled)
            {
                dg_updatableServers.Focus();
            }
        }

        private static Task updateTask = null;
        private static string updateLog = string.Empty;

        private async void btn_runUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (config.validateUpdates != chk_validate.IsChecked.Value)
            {
                config.validateUpdates = chk_validate.IsChecked.Value;
                config.Save();
            }

            if (updateTask == null || updateTask.Status is not TaskStatus.Running or TaskStatus.WaitingToRun)
            {
                HashSet<int> serverIDsToUpdate = dg_updatableServers.SelectedItems.Cast<UpdatableServerListEntry>().Select(s => s.targetServerID).ToHashSet();
                
                updateTask = Task.Factory.StartNew(() => { UpdateButtonTask(serverIDsToUpdate); });

                btn_runUpdate.IsEnabled = false;
            }
        }

        private void UpdateButtonTask(HashSet<int> serverIDsToUpdate)
        {
            try
            {
                UpdateMultipleServers(serverIDsToUpdate);

            }
            catch (Exception e)
            {
                WriteToUpdateOutput(e.ToString());
            }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    btn_runUpdate.IsEnabled = true;
                });
            }
        }

        private void UpdateMultipleServers(HashSet<int> serverIDsToUpdate)
        {
            if (serverIDsToUpdate == null)
            {
                throw new ArgumentNullException();
            }

            WriteToUpdateOutput($"Updating {serverIDsToUpdate.Count} servers");
            int i = 0;
            int erroredUpdates = 0;
            foreach (int serverID in serverIDsToUpdate)
            {
                i++;
                WriteToUpdateOutput($"Updating server {i} of {serverIDsToUpdate.Count}");
                int updaterExitCode = UpdateSingleServer(serverID);

                if (updaterExitCode != 0)
                {
                    erroredUpdates++;
                }
            }

            WriteToUpdateOutput($"Finished updating servers");
            if (erroredUpdates > 0)
            {
                WriteToUpdateOutput("Some servers may have failed to update. Please check the above log.");
            }
        }

        private int UpdateSingleServer(int targetServerID, bool downloadDepotDownloader = false)
        {
            string depotDownloaderExePath = Path.Combine(config.depotDownloaderFolder, config.depotDownloaderExe);
            if (!File.Exists(depotDownloaderExePath))
            {
                MessageBoxResult boxResult = MessageBox.Show("DepotDownloader has not yet been downloaded. This tool is required to perform updates. \n Would you like to download it to continue with the update?", "Unable to locate DepotDownloader", MessageBoxButton.YesNo);

                if (boxResult == MessageBoxResult.Yes)
                {
                    WriteToUpdateOutput("Downloading DepotDownloader");
                    Directory.CreateDirectory(config.depotDownloaderFolder);

                    string zipFilePath = Path.Combine(config.depotDownloaderFolder, "DepotDownloader.zip");
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(config.depotDownloaderURL, zipFilePath);
                    }
                    WriteToUpdateOutput("Extracting DepotDownloader");
                    ZipFile.ExtractToDirectory(zipFilePath, config.depotDownloaderFolder);
                }
            }

            ASCTServerConfig targetServer = config.Servers.Where(s => s.ID == targetServerID).FirstOrDefault();

            WriteToUpdateOutput($"Updating: \"{targetServer.Name}\" in {targetServer.GameDirectory} ");

            bool serverWasRunning = targetServer.IsRunning;

            int serverLockId = targetServer.ProcessManager.LockServer("Update in progress");

            if (serverWasRunning)
            {
                WriteToUpdateOutput($"Stopping server...");
                targetServer.ProcessManager.Stop();
                WriteToUpdateOutput($"Server stopped.");
            }

            Process updateProcess = new Process();
            updateProcess.StartInfo = new ProcessStartInfo
            {
                FileName = depotDownloaderExePath,
                Arguments = $"-app {config.serverAppID} -dir \"{targetServer.GameDirectory}\" {(config.validateUpdates ? "-validate" : string.Empty)}",
                RedirectStandardOutput = false,
                CreateNoWindow = false,
                UseShellExecute = false
            };

            updateProcess.Start();
            updateProcess.WaitForExit();

            if (updateProcess.ExitCode != 0)
            {
                WriteToUpdateOutput($"Update may have failed, exit code: {updateProcess.ExitCode}");
            }

            if (serverWasRunning)
            {
                WriteToUpdateOutput($"Starting server...");
                targetServer.ProcessManager.Start();
                WriteToUpdateOutput($"Started server.");
            }

            targetServer.ProcessManager.UnlockServer(serverLockId);

            return updateProcess.ExitCode;
        }

        private void WriteToUpdateOutput(string message)
        {
            Dispatcher.Invoke(() =>
            {
                updateLog += message + Environment.NewLine;

                txt_updateConsole.Text = updateLog;
                ConsoleScrollViewer.ScrollToBottom();
            });
        }

        private void dg_updatableServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtons();
        }
    }

    class UpdatableServerListEntry
    {
        public UpdatableServerListEntry(int serverID)
        {
            targetServerID = serverID;
        }

        private ASCTServerConfig targetServer
        {
            get
            {
                return ASCTGlobalConfig.Instance.Servers.Where(s => s.ID == targetServerID).DefaultIfEmpty(null).FirstOrDefault();
            }
        }

        public int targetServerID { get; private set; }

        public string serverName => targetServer.Name;
        public string serverCluster => targetServer.ClusterKey;
        public string serverRunning => targetServer.IsRunningToString;
    }
}

