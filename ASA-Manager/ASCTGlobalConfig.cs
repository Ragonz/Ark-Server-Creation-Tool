using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ARKServerCreationTool
{
    public class ASCTGlobalConfig
    {
        [JsonIgnore]
        public static ASCTGlobalConfig _instance = null;

        [JsonIgnore]
        public static ASCTGlobalConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = LoadConfig();

                return _instance;
            }
        }

        public static void NewConfig()
        {
            if (Instance == null)
            {
                _instance = new ASCTGlobalConfig();
            }
        }

        [JsonIgnore]
        public const string configName = "ASCTGlobalConfig.json";

        [JsonIgnore]
        public const string relativeEXEPath = @"ShooterGame\Binaries\Win64\ArkAscendedServer.exe";
        [JsonIgnore]
        public string depotDownloaderFolder = "depotdownloader";
        [JsonIgnore]
        public string depotDownloaderURL = @"https://github.com/SteamRE/DepotDownloader/releases/download/DepotDownloader_2.6.0/DepotDownloader-framework.zip";
        [JsonIgnore]
        public string depotDownloaderExe = "DepotDownloader.exe";
        [JsonIgnore]
        public ulong serverAppID = 2430930;
        [JsonIgnore]
        public static readonly string[] maps = new string[]
        {
            "TheIsland_WP", "ScorchedEarth_WP", "thecenter_wp"
        };

        public string ServersInstallationPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "InstalledServers");
        public string GlobalClusterDir { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "InstalledServers", "ClusterData");

        public bool AutomaticallyCreateFirewallRule { get; set; } = true;

        public ushort StartingGamePort { get; set; } = 7777;
        public ushort StartingRCONPort { get; set; } = 27015;

        public ushort PortIncrement { get; set; } = 10;

        public bool validateUpdates = true;

        public List<ASCTServerConfig> Servers = new List<ASCTServerConfig>();

        public ushort NextAvailablePort()
        {
            return (ushort)(Servers.Select(s => s.GamePort).DefaultIfEmpty((ushort)(StartingGamePort - PortIncrement)).Max() + PortIncrement);
        }

        public int NextAvailableID()
        {
            return Servers.Select(s => s.ID).DefaultIfEmpty(-1).Max() + 1;
        }

        public void Save()
        {
            SaveConfig(this);
        }

        private static void SaveConfig(ASCTGlobalConfig config)
        {
            File.WriteAllText(configName, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        public static ASCTGlobalConfig LoadConfig()
        {
            ASCTGlobalConfig returnConfig = null;

            if (File.Exists(configName))
            {
                string json = File.ReadAllText(configName);

                returnConfig = JsonConvert.DeserializeObject<ASCTGlobalConfig>(json);
            }

            return returnConfig;
        }
    }

    public class ASCTServerConfig
    {
        public ASCTServerConfig(int ID, ushort GamePort)
        {
            this.ID = ID;
            this.Name = $"ASA Server {ID}";
            this.GameDirectory = $"ASA Server {ID}";
            this.GamePort = GamePort;
        }

        internal GameProcessManager ProcessManager => GameProcessManager.GetGameProcessManager(this.ID);

        public bool IsRunning => ProcessManager.IsRunning;
        public string IsRunningToString => IsRunning ? "Running" : "Stopped";

        public int ID { get; private set; } //to be used as a primary key
        public string Name { get; set; } //friendly name for identifying the server to the user

        public string ClusterKey { get; set; } = string.Empty; //The cluster targetServerID 

        public string GameDirectory { get; set; } //Path to where the server is located
        public string EXEPath { get { return Path.Combine(GameDirectory, ASCTGlobalConfig.relativeEXEPath); } }

        public bool UseMultihome { get; set; } = false; //Whether the server should be launched with the MultihomeArgs argument
        public string IPAddress { get; set; } = string.Empty; //The IP address to pass to multihome
        public ushort GamePort { get; set; } //main gameport

        public string Map { get; set; } = "TheIsland_WP"; //Map that will be loaded

        public bool useCustomLaunchArgs { get; set; } = false; //Whether to use the user provided launch arguments
        public string customLaunchArgs { get; set; } = string.Empty; //user provided launch arguments

        public bool NoBattleye { get; set; } = false;

        public uint Slots { get; set; } = 70;

        public bool AllowCrossplay { get; set; } = false;

        public HashSet<ulong> modIDs = new HashSet<ulong>();

        [JsonIgnore]
        public string LaunchArguments
        {
            get
            {
                if (useCustomLaunchArgs)
                {
                    return customLaunchArgs;
                }
                else
                {
                    return $"\"{Map}{MultihomeArgs}\" \"-port={GamePort}\" -WinLiveMaxPlayers={Slots}{ModArgs}{ClusterArgs}{CrossplayArgs}{NoBattleyeArgs} -log -servergamelog".Trim();
                }
            }
        }

        [JsonIgnore]
        public string NoBattleyeArgs
        {
            get
            {
                return NoBattleye ? " -NoBattlEye" : string.Empty;
            }
        }

        [JsonIgnore]
        public string ModArgs
        {
            get
            {
                if (modIDs.Count <= 0)
                {
                    return string.Empty;
                }

                return $" \"-mods={string.Join(",", modIDs)}\"";
            }
        }

        [JsonIgnore]
        public string ClusterArgs
        {
            get
            {
                if (ClusterKey == string.Empty)
                {
                    return string.Empty;
                }

                return $" \"-clusterid={ClusterKey}\" \"-ClusterDirOverride={ASCTGlobalConfig.Instance.GlobalClusterDir}\"";
            }
        }

        [JsonIgnore]
        public string MultihomeArgs
        {
            get
            {
                if (UseMultihome && IPAddress != string.Empty)
                {
                    return $"?MultiHome={this.IPAddress}";
                }

                else
                {
                    return string.Empty;
                }
            }
        }

        [JsonIgnore]
        public string CrossplayArgs
        {
            get
            {
                return AllowCrossplay ? " -ServerPlatform=ALL" : string.Empty;
            }
        }
    }
}
