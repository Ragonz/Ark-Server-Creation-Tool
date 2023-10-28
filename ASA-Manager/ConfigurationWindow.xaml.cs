using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        private bool firstLaunch = false;
        public ConfigurationWindow(bool firstLaunch = false)
        {
            InitializeComponent();

            txt_gameDir.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory;

            txt_steamUsername.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).SteamUsername;
            txt_steamPassword.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).SteamPassword;

            this.firstLaunch = firstLaunch;
        }

        private void btn_saveConfig_Click(object sender, RoutedEventArgs e)
        {
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory = txt_gameDir.Text;

            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).SteamUsername = txt_steamUsername.Text;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).SteamPassword = txt_steamPassword.Text;

            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).Save();

            if (firstLaunch)
            {
                UpdaterWindow update = new UpdaterWindow(true);

                update.Show();
            }

            this.Close();
        }
        private void btn_gameDirBrowse_Click(object sender, RoutedEventArgs e)
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
    }
}
