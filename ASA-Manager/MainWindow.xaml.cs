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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStatus == RunButtonStatus.Run)
            {
                GameProcessManager.Start();
            }
            else if (buttonStatus == RunButtonStatus.Stop)
            {
                GameProcessManager.Stop();
            }

            SetButtonStatus();
        }

        private void SetButtonStatus()
        {
            if (GameProcessManager.IsRunning)
            {
                buttonStatus = RunButtonStatus.Stop;
                btn_run.Content = "Stop Server";
            }
            else
            {
                buttonStatus = RunButtonStatus.Run;
                btn_run.Content = "Start Server";
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
    }
}
