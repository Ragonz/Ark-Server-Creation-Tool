﻿using System;
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

namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UpdaterWindow : Window
    {
        Task updateTask;

        bool firstLaunch;

        public UpdaterWindow(bool firstLaunch = false)
        {
            InitializeComponent();

            if (firstLaunch)
            {
                txt_updateConsole.Text = "Click \"Update Game Files\" to download the game.";
                btn_exit.Content = "Continue";
                btn_exit.IsEnabled = false;
            }

            this.firstLaunch = firstLaunch;
        }

        private async void btn_runUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUpdating())
            {
                btn_runUpdate.IsEnabled = false;
                txt_updateConsole.Text = string.Empty;
                updateTask = Task.Factory.StartNew(() => { Update(); });                
            }
        }

        private bool IsUpdating()
        {           
            return updateTask != null && updateTask.Status == TaskStatus.Running;
        }

        List<string> consoleBatchQueue = new List<string>();

        private void Update()
        {
            try
            {
                consoleBatchQueue.Clear();

                string depotDownloaderFolder = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).depotDownloaderFolder;
                string depotDownloaderURL = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).depotDownloaderURL;
                string depotDownloaderExe = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).depotDownloaderExe;

                string directoryToUpdate = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory;
                ulong serverAppID = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).serverAppID;

                AddToConsole($"Updating Game Files in {directoryToUpdate} - {DateTime.Now:HH:mm:ss}");

                if (Directory.Exists("DepotDownloader"))
                {
                    AddToConsole($"Clearing old files");
                    Directory.Delete("DepotDownloader", true);
                }

                Directory.CreateDirectory(depotDownloaderFolder);

                string zipFilePath = Path.Combine(depotDownloaderFolder, "DepotDownloader.zip");

                using (WebClient wc = new WebClient())
                {
                    AddToConsole($"Downloading Depot Downloader");
                    wc.DownloadFile(depotDownloaderURL, zipFilePath);
                }

                AddToConsole($"Extracting Depot Downloader");
                ZipFile.ExtractToDirectory(zipFilePath, depotDownloaderFolder);

                string depotDownloaderExePath = Path.Combine(depotDownloaderFolder, depotDownloaderExe);
                if (!File.Exists(depotDownloaderExePath))
                {
                    ExitUpdateMessage($"ERROR: Could not locate{depotDownloaderExe} - Update aborted");
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = depotDownloaderExePath,
                    Arguments = $"-app {serverAppID} -dir {directoryToUpdate} -validate",
                    RedirectStandardOutput = false,
                    CreateNoWindow = false,
                    UseShellExecute = false
                };

                Process p = new Process();
                p.StartInfo = startInfo;

                p.OutputDataReceived += (s, e) => { AddToConsole(e.Data); };

                p.Start();
               // p.BeginOutputReadLine();
                p.WaitForExit();

            }
            catch (Exception e)
            {
                ExitUpdateMessage(e.Message + Environment.NewLine + e.StackTrace);
            }
            ExitUpdateMessage("Update Complete");
        }

        private void ExitUpdateMessage(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                txt_updateConsole.Text += message + Environment.NewLine;
                btn_runUpdate.IsEnabled = true;
                btn_exit.IsEnabled = true;
            });
        }
        
        private delegate void AddToConsoleCallBack(string message);

        Stopwatch timer = new Stopwatch();

        private void AddToConsole(string message)
        {
            bool sendConsoleQueueToView = false;

            sendConsoleQueueToView = (!timer.IsRunning || timer.ElapsedMilliseconds >= 1000);

            consoleBatchQueue.Add(message);

            if (sendConsoleQueueToView)
            {
                for (int i = 0; i < consoleBatchQueue.Count; i++)
                {
                    Dispatcher.Invoke(() =>
                    {
                        txt_updateConsole.Text += string.Join(Environment.NewLine, consoleBatchQueue);
                        ConsoleScrollViewer.ScrollToBottom();
                    });
                    timer.Restart();
                    consoleBatchQueue.Clear();
                }
            }
        }

        bool alreadyReturning = false;

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            if (IsUpdating())
            {
                MessageBox.Show("An update is currently running. Exiting now could cause issues. Please wait for it to finish.");
            }
            else
            {
                BackToMain();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsUpdating())
            {
                MessageBox.Show("An update is currently running. Exiting now could cause issues. Please wait for it to finish.");
                e.Cancel = true;
            }
            else 
            {
                BackToMain(false);
            }
        }

        private void BackToMain(bool triggerClose = true)
        {
            try
            {
                if (alreadyReturning)
                {
                    return;
                }

                alreadyReturning = true;

                MainWindow main = null;

                main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

                if (main != null)
                {
                    main.Activate();
                }
                else
                {
                    main = new MainWindow();
                    main.Show();
                }

                if (triggerClose) this.Close();
            }
            catch (Exception e )
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }
    }
}