using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsFirewallHelper;

namespace ARKServerCreationTool
{
    internal class GameProcessManager
    {
        static Random rng = new Random();

        private static Dictionary<int, GameProcessManager> managers = new Dictionary<int, GameProcessManager>();
        private Dictionary<int, string> serverLocks = new Dictionary<int, string>();

        public int targetServerID { get; private set; }

        private ASCTServerConfig targetServer { get { return ASCTGlobalConfig.Instance.Servers[targetServerID]; } }

        private GameProcessManager(int targetServerID)
        {
            this.targetServerID = targetServerID;
        }

        public static GameProcessManager GetGameProcessManager(int targetServerID)
        {
            if (!managers.Keys.Contains(targetServerID))
            {
                managers.Add(targetServerID, new GameProcessManager(targetServerID));
            }
            return managers[targetServerID];

        }

        private Process gameProcess;

        public int LockServer(string LockMessage)
        {
            int lockID = rng.Next();

            serverLocks.Add(lockID, LockMessage);

            return lockID;
        }

        public void UnlockServer(int lockID)
        {
            serverLocks.Remove(lockID);
        }

        public bool IsRunning
        {
            get
            {
                string exePath = targetServer.EXEPath;

                if (gameProcess != null && gameProcess.Id > 0 && !gameProcess.HasExited)
                {
                    return true;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(targetServer.EXEPath);
                    Process[] processes = Process.GetProcessesByName(fileName);

                    gameProcess = null;

                    for (int i = 0; i < processes.Length; i++)
                    {
                        try
                        {
                            if (processes[i].MainModule.FileName.Equals(exePath, StringComparison.OrdinalIgnoreCase))
                            {
                                gameProcess = processes[i];
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            //Won'T be able to access MainModule of 32bit programs
                        }
                    }

                    return gameProcess != null;
                }
            }
        }

        public bool Start()
        {
            try
            {
                bool success = false;
                if (IsRunning)
                {
                    success = true; ;
                }
                else if (serverLocks.Count != 0)
                {
                    MessageBox.Show($"{targetServer.Name} is locked: \n{string.Join("\n", serverLocks.Select(l => l.Value))}", "Unable to start locked server");
                }
                else
                {  
                    gameProcess = new Process();
                    ProcessStartInfo si = new ProcessStartInfo();
                    si.UseShellExecute = false;
                    si.FileName = targetServer.EXEPath;
                    si.Arguments = targetServer.LaunchArguments;
                    si.WorkingDirectory = targetServer.GameDirectory;

                    gameProcess.StartInfo = si;

                    success = gameProcess.Start();

                    if (!success)
                    {
                        gameProcess = null;
                    }

                    if (ASCTGlobalConfig.Instance.AutomaticallyCreateFirewallRule && !FirewallManager.Instance.Rules.Any(r => r.IsEnable && r.Direction ==  FirewallDirection.Inbound && r.Action == FirewallAction.Allow && r.ApplicationName != null && r.ApplicationName.Equals(targetServer.EXEPath, StringComparison.OrdinalIgnoreCase)))
                    {
                        try
                        {
                            var rule = FirewallManager.Instance.CreateApplicationRule(
                                @"Rule for ARK SA (Created by ASCT)",
                                FirewallAction.Allow,
                                targetServer.EXEPath
                            );
                            rule.Direction = FirewallDirection.Inbound;
                            FirewallManager.Instance.Rules.Add(rule);

                            MessageBox.Show("No existing Firewll Rule was detected, a new one was created.");
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"ASCT was unable to create the firewall rule. \n" +
                                $"You may be unable to connect to the server remotely ", "Error creating Firewall Rule");
                        }
                    }
                }

                return success;
            }
            catch (Exception e)
            {
                gameProcess = null;
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }            
        }


        public bool Stop()
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
