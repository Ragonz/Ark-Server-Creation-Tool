using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for ServerConfigurationWindow.xaml
    /// </summary>
    public partial class ServerConfigurationWindow : Window
    {
        public bool newServer { get; private set; } = false;
        private ASCTServerConfig targetServer;
        public int? targetServerID { get { return newServer ? null : targetServer.ID; } }

        ASCTGlobalConfig config = ASCTGlobalConfig.Instance;


        bool windowReady = false;
        public ServerConfigurationWindow(int? targetServerID = null)
        {

            if (targetServerID == null)
            {
                newServer = true;
                targetServer = new ASCTServerConfig(config.NextAvailableID(), config.NextAvailablePort());
                targetServer.GameDirectory = Path.Combine(config.ServersInstallationPath, targetServer.Name);
            }
            else
            {
                targetServer = config.Servers.First(s => s.ID == targetServerID.Value);
            }

            InitializeComponent();

            txt_serverName.Text = targetServer.Name;
            txt_gameDir.Text = targetServer.GameDirectory;
            chkbx_overrideCommandline.IsChecked = targetServer.useCustomLaunchArgs;
            txt_commandLine.IsEnabled = targetServer.useCustomLaunchArgs;
            txt_commandLine.Text = targetServer.LaunchArguments;
            ValidateGamePortString(txt_gamePort.Text = targetServer.GamePort.ToString());

            UpdateClusterCombo();
            UpdateMapCombo();
            UpdateCommandLineBox();
            UpdateModList();

            windowReady = true;
        }

        private void UpdateModList() 
        {
            lst_modIds.Items.Clear();

            foreach (var item in targetServer.modIDs)
            {
                lst_modIds.Items.Add(item);
            }

            lst_modIds.Items.Refresh();
        }

        private void UpdateClusterCombo()
        {
            cmbo_clusterKey.Items.Clear();
            cmbo_clusterKey.Items.Add(string.Empty);

            foreach (var item in config.Servers.Select(s => s.ClusterKey).Distinct().Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                cmbo_clusterKey.Items.Add(item);
            }

            cmbo_clusterKey.SelectedItem = targetServer.ClusterKey;

            cmbo_clusterKey.Items.Refresh();
        }

        private void UpdateMapCombo()
        {
            cmbo_Map.Items.Clear();

            foreach (string map in config.Servers.Select(s => s.Map).Union(ASCTGlobalConfig.maps).Distinct().Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                cmbo_Map.Items.Add(map);
            }

            cmbo_Map.SelectedItem = targetServer.Map;

            cmbo_Map.Items.Refresh();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (config.Servers.Any(s => s.ID != targetServer.ID && s.GameDirectory.Equals(txt_gameDir.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Another server is already using this directory. ASCT does not support this.");
                return;
            }

            if (txt_gameDir.Text != targetServer.GameDirectory && Directory.Exists(targetServer.GameDirectory))
            {
                var result = MessageBox.Show($"Changing the directory will not move the existing files in {Environment.NewLine}{targetServer.GameDirectory}{Environment.NewLine}Are you sure you wish to continue?", "Are you sure?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            UpdateServerObject(ref targetServer);

            if (newServer)
            {
                config.Servers.Add(targetServer);
            }

            config.Save();

            if (newServer)
            {
                ASCTTools.FindOrCreateServerUpdaterWindow(targetServer.ID);
                Close();
            }
            else
            {
                ASCTTools.FindOrCreateServerWindow(targetServer.ID);
                Close();
            }
        }

        private void UpdateServerObject(ref ASCTServerConfig serv)
        {
            if (!ValidateGamePortString(txt_gamePort.Text))
            {
                return;
            }

            serv.Name = txt_serverName.Text;
            serv.GameDirectory = txt_gameDir.Text;
            serv.ClusterKey = cmbo_clusterKey.Visibility == Visibility.Visible ? (string)cmbo_clusterKey.SelectedItem : txt_newClusterID.Text.Trim();
            serv.Map = cmbo_Map.Visibility == Visibility.Visible ? (string)cmbo_Map.SelectedItem : txt_customMap.Text.Trim();
            serv.useCustomLaunchArgs = chkbx_overrideCommandline.IsChecked.Value;
            serv.GamePort = ushort.Parse(txt_gamePort.Text);
            serv.modIDs = lst_modIds.Items.Cast<ulong>().ToHashSet();
            if (chkbx_overrideCommandline.IsChecked.Value) serv.customLaunchArgs = txt_commandLine.Text.Trim();
        }

        private void btn_newCluster_Click(object sender, RoutedEventArgs e)
        {
            cmbo_clusterKey.Visibility = cmbo_clusterKey.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            txt_newClusterID.Visibility = txt_newClusterID.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            btn_newCluster.Content = cmbo_clusterKey.Visibility == Visibility.Visible ? "New Cluster..." : "Existing Cluster...";
            UpdateClusterCombo();
        }

        private void btn_newMap_Click(object sender, RoutedEventArgs e)
        {
            cmbo_Map.Visibility = cmbo_Map.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            txt_customMap.Visibility = txt_customMap.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            btn_newMap.Content = cmbo_Map.Visibility == Visibility.Visible ? "New Map..." : "Existing Map...";
            UpdateClusterCombo();
        }

        ASCTServerConfig copyObject = null;

        private void UpdateCommandLineBox()
        {
            if (!chkbx_overrideCommandline.IsChecked.Value && windowReady && targetServer != null)
            {
                if (copyObject == null) copyObject = new ASCTServerConfig(targetServer.ID, targetServer.GamePort);

                UpdateServerObject(ref copyObject);

                txt_commandLine.Text = copyObject.LaunchArguments; 
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCommandLineBox();
        }

        private void comboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCommandLineBox();
        }

        private void chkbx_overrideCommandline_Click(object sender, RoutedEventArgs e)
        {
            txt_commandLine.IsEnabled = chkbx_overrideCommandline.IsChecked.Value;

            UpdateCommandLineBox();
        }

        Regex numberRegex = new Regex("^[0-9]+$", RegexOptions.Compiled);
        private bool ValidateGamePortString(string portString)
        {
            bool valid = ushort.TryParse(portString, out _) && numberRegex.Match(portString).Success;

            lbl_invalidPort.Visibility = valid ? Visibility.Collapsed : Visibility.Visible;

            return valid;
        }

        private void txt_gamePort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidateGamePortString(txt_gamePort.Text + e.Text);
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.InitialDirectory = txt_gameDir.Text;

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txt_gameDir.Text = dialog.SelectedPath;
                }
            }
        }

        private void btn_addMod_Click(object sender, RoutedEventArgs e)
        {
            ulong modID = 0;

            if (ulong.TryParse(txt_addMod.Text, out modID))
            {
                targetServer.modIDs.Add(modID);
                UpdateModList();                
            }
            else
            {
                MessageBox.Show("Entered value is invalid");
            }
            UpdateCommandLineBox();
        }

        private void lst_modIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_removeMod.IsEnabled = lst_modIds.SelectedItems.Count > 0;
        }

        private void btn_removeMod_Click(object sender, RoutedEventArgs e)
        {
            targetServer.modIDs.RemoveWhere(x => lst_modIds.SelectedItems.Cast<ulong>().Contains(x));
            UpdateModList();
            UpdateCommandLineBox();

        }
    }
}
