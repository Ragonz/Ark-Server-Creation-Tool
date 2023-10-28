using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace ARKServerCreationTool
{
    public class ASCTConfiguration
    {
        [JsonIgnore]
        public const string configName = "ASCTConfiguration.json";
        [JsonIgnore]
        public const string relativeEXEPath = @"ShooterGame\Binaries\Win64\ArkAscendedServer.exe";
        [JsonIgnore]
        public  string depotDownloaderFolder = "depotdownloader";
        [JsonIgnore]
        public  string depotDownloaderURL = @"https://github.com/SteamRE/DepotDownloader/releases/download/DepotDownloader_2.5.0/depotdownloader-2.5.0.zip";
        [JsonIgnore]
        public  string depotDownloaderExe = "DepotDownloader.exe";
        [JsonIgnore]
        public ulong serverAppID = 2430930;

        public string GameDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ARK_SA");

        public ushort GamePort = 27015;
        public ushort QueryPort = 27016;
        public ushort RConPort = 27017;
        public ushort PeerPort = 27018;

        public string SteamUsername;
        public string SteamPassword;
        public bool SteamClientAutoLaunch = true;

        [JsonIgnore]
        public string ExecutablePath { get { return Path.Combine(GameDirectory, relativeEXEPath); } }
        [JsonIgnore]
        public string LaunchArguments { get { return $"\"TheIsland_WP?MultiHome={GetLocalIPAddress()}?QueryPort={QueryPort}?RCONEnabled=True?RCONServerGameLogBuffer=600?RCONPort={RConPort}?MaxPlayers=47?listen\" -port={GamePort} -PeerPort={PeerPort} -game -server -log -servergamelog -ForceAllowCaveFlyers -newsaveformat -usestore"; } }

        public void Save()
        {
            SaveConfig(this);
        }

        public static void SaveConfig(ASCTConfiguration config)
        {
            File.WriteAllText(configName, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        public static ASCTConfiguration LoadConfig()
        {
            ASCTConfiguration returnConfig = null;

            if (File.Exists(configName))
            {
                string json = File.ReadAllText(configName);

                returnConfig = JsonConvert.DeserializeObject<ASCTConfiguration>(json);
            }

            return returnConfig;
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
