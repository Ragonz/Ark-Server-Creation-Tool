using System;
using System.Collections.Generic;
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
            chk_overrideLaunchArgs.IsChecked = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).useCustomLaunchArgs;
            txt_customLaunchArgs.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).customLaunchArgs;
            txt_Game_Port.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).overrideGamePort == null ? string.Empty : ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).overrideGamePort.Value.ToString();
            chk_useMultihome.IsChecked = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).UseMultihome;
            txt_IPAddress.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).IPAddress;

            this.firstLaunch = firstLaunch;
        }

        private void btn_saveConfig_Click(object sender, RoutedEventArgs e)
        {
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory = txt_gameDir.Text.Trim();
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).useCustomLaunchArgs = chk_overrideLaunchArgs.IsChecked.Value;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).customLaunchArgs = txt_customLaunchArgs.Text.Trim();
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).overrideGamePort = txt_Game_Port.Text.Trim() == string.Empty ? null : ushort.Parse(txt_Game_Port.Text.Trim());
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).UseMultihome = chk_useMultihome.IsChecked.Value;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).IPAddress = txt_IPAddress.Text;

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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
           
           e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
