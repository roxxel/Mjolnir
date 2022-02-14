using MonoTorrent.Client;
using SuRGeoNix.BitSwarmLib;
using System.Net;
using System.Net.NetworkInformation;

namespace Mjolnir.Core
{
    public class TorrentClient
    {
        private StandardDownloader _downloader;

        public TorrentClient(string downloadsPath)
        {
            DownloadsPath = downloadsPath;
        }

        public string DownloadsPath { get; }

        public async Task InitializeAsync()
        {
            var settingBuilder = new EngineSettingsBuilder()
            {
                AllowPortForwarding = true,
                AutoSaveLoadDhtCache = true,
                AutoSaveLoadFastResume = true,
                AutoSaveLoadMagnetLinkMetadata = true,
                ListenPort = GetAvailablePort(40000),
                DhtPort = GetAvailablePort(40000)
            };

            var engine = new ClientEngine(settingBuilder.ToSettings());
            var downloader = new StandardDownloader(engine);
            _downloader = downloader;
        }

        public async Task<TorrentConnection> StartDownloadingAsync(string fileName, string downloadsPath)
        {
            return await _downloader.DownloadAsync(fileName, downloadsPath);
        }

        private int GetAvailablePort(int startingPort)
        {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            for (int i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }
    }
}