using Mjolnir.Core.EventArgs;
using MonoTorrent;
using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.Core
{
    public class TorrentConnection
    {
        private TorrentManager _manager;

        public TorrentConnection(TorrentManager manager)
        {
            _manager = manager;
            _manager.TorrentStateChanged += (sender, args) => StateChanged?.Invoke(this, new(args.OldState, args.NewState));
            _ = UpdateDataAsync();
        }

        public Torrent Torrent => _manager.Torrent;
        public TorrentState State => _manager.State;

        public event EventHandler<ConnectionStatsUpdatedEventArgs> StatsUpdated;
        public event EventHandler<ConnectionStateChangedEventArgs> StateChanged;

        public async Task StopAsync()
        {
            await _manager.StopAsync();
        }

        public async Task PauseAsync()
        {
            await _manager.PauseAsync();
        }

        private async Task UpdateDataAsync()
        {
            while (_manager != null)
            {
                if (_manager.State == TorrentState.Stopped || _manager.State == TorrentState.Paused || _manager.State == TorrentState.Error)
                {
                    await Task.Delay(500);
                    continue;
                }
                var mon = _manager.Monitor;
                var args = new ConnectionStatsUpdatedEventArgs(
                    mon.DataBytesDownloaded,
                    mon.DataBytesUploaded,
                    mon.DownloadSpeed,
                    mon.ProtocolBytesDownloaded,
                    mon.ProtocolBytesUploaded,
                    mon.UploadSpeed);

                StatsUpdated?.Invoke(this, args);

                await Task.Delay(1500);
            }
        }
    }
}
