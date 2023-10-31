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
        [JsonIgnore]
        public const ushort defaultGamePort = 7777;
        [JsonIgnore]
        public const string multihomeArgument = @"?MultiHome=";

        public string GameDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ARK_SA");

        public ushort? overrideGamePort = null;

        public bool UseMultihome = false;
        public string IPAddress = GetLocalIPAddress();

        [JsonIgnore]
        public string Multihome
        {
            get
            {
                if (UseMultihome)
                {
                    return multihomeArgument + IPAddress;
                }

                else
                {
                    return string.Empty;
                }
            }
        }

        [JsonIgnore]
        public ushort GamePort
        {
            get
            {
                if (overrideGamePort != null)
                {
                    return overrideGamePort.Value;
                }
                else
                {
                    return defaultGamePort;
                }
            }
        }

        [JsonIgnore]
        public string ExecutablePath { get { return Path.Combine(GameDirectory, relativeEXEPath); } }

        public bool useCustomLaunchArgs = false;
        public string customLaunchArgs = string.Empty;
        [JsonIgnore]
        public string LaunchArguments
        {
            get
            {
                if(useCustomLaunchArgs)
                {
                    return customLaunchArgs;
                }
                else
                {
                    return $"\"TheIsland_WP{Multihome}\" -port={GamePort}";
                }
            }
        }


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
