using SuRGeoNix.BitSwarmLib;

namespace Mjolnir.Core
{
    public class TorrentClient
    {
        private BitSwarm _bitSwarm;

        public TorrentClient(string torrent)
        {
            _bitSwarm = new BitSwarm(null);
            _bitSwarm.Open(torrent);
        }

        public async Task StartDownloading()
        {
            _ = Task.Run(() =>
            {
                _bitSwarm.Start();
            });
        }
    }
}