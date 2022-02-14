using MonoTorrent.Client;

namespace Mjolnir.Core.EventArgs
{
    public class ConnectionStateChangedEventArgs
    {
        public ConnectionStateChangedEventArgs(TorrentState previousState, TorrentState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }

        public TorrentState PreviousState { get; set; }
        public TorrentState CurrentState { get; set; }
    }
}