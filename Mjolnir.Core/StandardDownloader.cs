using MonoTorrent;
using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.Core
{
    public class StandardDownloader
    {
        private List<TorrentConnection> _managers;
        private ClientEngine _engine;
        public StandardDownloader(ClientEngine engine)
        {
            _engine = engine;
            _managers = new();
        }


        public async Task<TorrentConnection> DownloadAsync(string filePath, string downloadsPath)
        {
            var settingsBuilder = new TorrentSettingsBuilder
            {
                MaximumConnections = 60,
            };
            var mngr = await _engine.AddAsync(filePath, downloadsPath, settingsBuilder.ToSettings());
            var conn = new TorrentConnection(mngr);
            _managers.Add(conn);
            await mngr.StartAsync();
            return conn;
        }
    }
}
