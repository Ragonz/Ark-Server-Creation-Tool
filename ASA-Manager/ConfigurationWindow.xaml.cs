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
            chk_autoFirewallRules.IsChecked = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).AutomaticallyCreateFirewallRule;

            txt_RCONIPAddress.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).RCONIPaddress;
            txt_RCONPort.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).RCONPort.ToString();
            txt_adminPassword.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).RCONPassword;

            this.firstLaunch = firstLaunch;
        }

        private void btn_saveConfig_Click(object sender, RoutedEventArgs e)
        {
            string ipAddressPattern = "(^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$)|((([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])))";
            if (!Regex.Match(txt_IPAddress.Text, ipAddressPattern).Success)
            {
                MessageBox.Show("Entered IP address is not valid.");
                return;
            }

            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory = txt_gameDir.Text.Trim();
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).useCustomLaunchArgs = chk_overrideLaunchArgs.IsChecked.Value;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).customLaunchArgs = txt_customLaunchArgs.Text.Trim();
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).overrideGamePort = txt_Game_Port.Text.Trim() == string.Empty ? null : ushort.Parse(txt_Game_Port.Text.Trim());
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).UseMultihome = chk_useMultihome.IsChecked.Value;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).IPAddress = txt_IPAddress.Text;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).AutomaticallyCreateFirewallRule = chk_autoFirewallRules.IsChecked.Value;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).RCONIPaddress = txt_RCONIPAddress.Text;
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).RCONPort = ushort.Parse(txt_RCONPort.Text);
            ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).RCONPassword = txt_adminPassword.Text;

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
