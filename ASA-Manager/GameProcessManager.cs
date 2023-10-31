using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsFirewallHelper;

namespace ARKServerCreationTool
{
    internal static class GameProcessManager
    {
        private static Process gameProcess;

        public static bool IsRunning
        {
            get
            {
                string exePath = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).ExecutablePath;

                if (gameProcess != null && gameProcess.Id > 0 && !gameProcess.HasExited)
                {
                    return true;
                }
                else
                {
                    Process[] processes = Process.GetProcesses();

                    for (int i = 0; i < processes.Length; i++)
                    {
                        try
                        {
                            if (processes[i].MainModule.FileName.Equals(exePath, StringComparison.OrdinalIgnoreCase))
                            {
                                gameProcess = processes[i];
                                return true;
                            }
                        }
                        catch (Exception)
                        {
                            //Won't be able to access MainModule of 32bit programs
                        }
                    }
                    return false;
                }
            }
        }

        public static bool Start()
        {
            try
            {
                bool success = false;
                if (IsRunning)
                {
                    success = true; ;
                }
                else
                {
                    gameProcess = new Process();
                    ProcessStartInfo si = new ProcessStartInfo();
                    si.UseShellExecute = false;
                    si.FileName = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).ExecutablePath;
                    si.Arguments = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).LaunchArguments;
                    si.WorkingDirectory = ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).GameDirectory;

                    gameProcess.StartInfo = si;

                    success = gameProcess.Start();

                    if (((ASCTConfiguration)Application.Current.Properties["globalConfig"]).AutomaticallyCreateFirewallRule && !FirewallManager.Instance.Rules.Any(rule => rule.ApplicationName.Equals(((ASCTConfiguration)Application.Current.Properties["globalConfig"]).ExecutablePath, StringComparison.OrdinalIgnoreCase)))
                    {
                        try
                        {
                            var rule = FirewallManager.Instance.CreateApplicationRule(
                                @"Rule for ARK SA (Created by ASCT)",
                                FirewallAction.Allow,
                                ((ASCTConfiguration)Application.Current.Properties["globalConfig"]).ExecutablePath
                            );
                            rule.Direction = FirewallDirection.Inbound;
                            FirewallManager.Instance.Rules.Add(rule);

                            MessageBox.Show("No existing Firewll Rule was detected, a new one was created.");
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"ASCT was unable to create the firewall rule. \n" +
                                $"You may be unable to connect to the service remotely ", "Error creating Firewall Rule");
                        }
                    }
                }

                return success;
            }
            catch (Exception)
            {
                return false;
            }            
        }


        public static bool Stop()
        {
            int tries = 0;
            int maxTries = 5;

            while (IsRunning && tries++ <= maxTries)
            {                
                gameProcess.Kill();
                Thread.Sleep(tries * 10);
            }

            return IsRunning;
        }
    }
}
