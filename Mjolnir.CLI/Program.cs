using Mjolnir.Core;

//For now it's only for testing purposes, later it'll be converted to fully working command line utility

var torrentClient = new TorrentClient("downloaded");
await torrentClient.InitializeAsync();
var torrent = await torrentClient.StartDownloadingAsync("microsoft-office-2016.torrent", "downloaded");
var torrent1 = await torrentClient.StartDownloadingAsync("microsoft-office.torrent", "downloaded");
torrent1.StatsUpdated += Torrent_StatsUpdated;

torrent.StatsUpdated += Torrent_StatsUpdated;

void Torrent_StatsUpdated(object? sender, Mjolnir.Core.EventArgs.ConnectionStatsUpdatedEventArgs e)
{
    var client = (TorrentConnection)sender;
    Console.WriteLine("Downloading: " + client.Torrent.Name);
    Console.WriteLine("State: " + client.State.ToString());
    Console.WriteLine();
    Console.WriteLine($"DataKBDownloaded: {e.DataBytesDownloaded / 1000}kb");
    Console.WriteLine($"DataBytesUploaded: {e.DataBytesUploaded}");
    Console.WriteLine($"DownloadSpeed: {e.DownloadSpeed}");
    Console.WriteLine($"ProtocolBytesDownloaded: {e.ProtocolBytesDownloaded}");
    Console.WriteLine($"ProtocolBytesUploaded: {e.ProtocolBytesUploaded}");
    Console.WriteLine($"UploadSpeed: {e.UploadSpeed}");
    Console.WriteLine("------------------------------\n");
}
Console.CancelKeyPress += Console_CancelKeyPress;

async void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
{
    e.Cancel = true;
    await torrent.StopAsync();
    Console.WriteLine("Press any key to exit");
    Console.ReadKey();
    Environment.Exit(0);
}

await Task.Delay(-1);
