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
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
using RconSharp;


namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum RunButtonStatus
        {
            Unknown, Run, Stop
        }

        RunButtonStatus buttonStatus = RunButtonStatus.Unknown;


        public MainWindow()
        {
            InitializeComponent();

            SetButtonStatus();
        }

        private async void btn_run_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStatus == RunButtonStatus.Run)
            {
                GameProcessManager.Start();
            }
            else if (buttonStatus == RunButtonStatus.Stop && GameProcessManager.IsRunning)
            {
                GameProcessManager.Shutdown();
            }

            SetButtonStatus();
        }

        private void btn_force_kill_Click(object sender, RoutedEventArgs e)
        {
            GameProcessManager.ForceStop();

            SetButtonStatus();
        }

        private void SetButtonStatus()
        {
            if (GameProcessManager.IsRunning)
            {
                buttonStatus = RunButtonStatus.Stop;
                btn_run.Content = "Shutdown Server";
                btn_force_kill.IsEnabled = true;
            }
            else
            {
                buttonStatus = RunButtonStatus.Run;
                btn_run.Content = "Start Server";
                btn_force_kill.IsEnabled = false;
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

        private void Window_Activated(object sender, EventArgs e)
        {
            SetButtonStatus();
        }

        private void btn_openConfigFolder_Click(object sender, RoutedEventArgs e)
        {
            string GUSConfigPath = Path.Combine(((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory, @"ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini");

            if (File.Exists(GUSConfigPath))
            {
                Process.Start("notepad.exe", GUSConfigPath).WaitForExit();
            }
            else
            {
                string message = "No config file currently exists. Would you like to create one from the template?";
                string caption = "Missing Config";
                MessageBoxResult result = MessageBox.Show(message, caption,
                              MessageBoxButton.YesNo );

                if (result == MessageBoxResult.Yes)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(GUSConfigPath));
                    File.WriteAllText(GUSConfigPath, Properties.Resources.GUSConfigTemplate);
                    Process.Start("notepad.exe", GUSConfigPath).WaitForExit();
                }
            }
        }
    }
}
