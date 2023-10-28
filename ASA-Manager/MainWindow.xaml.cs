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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            txt_Executable.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).ExecutablePath;
            txt_Arguments.Text = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).LaunchArguments;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = txt_Executable.Text,
                    Arguments = txt_Arguments.Text,
                    WorkingDirectory = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory,
                    RedirectStandardOutput = chk_stdout.IsChecked.Value,
                    RedirectStandardInput = chk_stdin.IsChecked.Value,
                    RedirectStandardError = chk_stderr.IsChecked.Value,
                    UseShellExecute = false
                };

                Process p = new Process();
                p.StartInfo = startInfo;

                p.Start();
                p.WaitForExit();

                if (chk_stdout.IsChecked.Value) File.WriteAllText("OUT.txt", p.StandardOutput.ReadToEnd());
                if (chk_stderr.IsChecked.Value) File.WriteAllText("ERR.txt", p.StandardError.ReadToEnd());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                throw;
            }
        }

        private void btn_openConfig_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow desiredWindow = null;

            desiredWindow = Application.Current.Windows.OfType<ConfigurationWindow>().FirstOrDefault();

            if (desiredWindow != null)
            {
                desiredWindow.Activate();
            }
            else
            {
                desiredWindow = new ConfigurationWindow();
                desiredWindow.Show();
            }
        }

        private void btn_openUpdater_Click(object sender, RoutedEventArgs e)
        {
            UpdaterWindow desiredWindow = null;

            desiredWindow = Application.Current.Windows.OfType<UpdaterWindow>().FirstOrDefault();

            if (desiredWindow != null)
            {
                desiredWindow.Activate();
            }
            else
            {
                desiredWindow = new UpdaterWindow();
                desiredWindow.Show();
            }
        }
    }
}
