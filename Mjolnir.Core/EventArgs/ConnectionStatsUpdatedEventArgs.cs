using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.Core.EventArgs
{
    public class ConnectionStatsUpdatedEventArgs
    {
        public long DataBytesDownloaded { get; set; }
        public long DataBytesUploaded { get; set; }
        public long DownloadSpeed { get; set; }
        public long ProtocolBytesDownloaded { get; set; }
        public long ProtocolBytesUploaded { get; set; }
        public long UploadSpeed { get; set; }

        public ConnectionStatsUpdatedEventArgs(
            long dataBytesDownloaded,
            long dataBytesUploaded,
            long downloadSpeed,
            long protocolBytesDownloaded,
            long protocolBytesUploaded,
            long uploadSpeed)
        {
            DataBytesDownloaded = dataBytesDownloaded;
            DataBytesUploaded = dataBytesUploaded;
            DownloadSpeed = downloadSpeed;
            ProtocolBytesDownloaded = protocolBytesDownloaded;
            ProtocolBytesUploaded = protocolBytesUploaded;
            UploadSpeed = uploadSpeed;
        }
    }
}
