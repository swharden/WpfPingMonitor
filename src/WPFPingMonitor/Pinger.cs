using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows.Threading;

namespace WPFPingMonitor
{
    public class Pinger
    {
        public ObservableCollection<PingResult> PingResults { get; set; } = new ObservableCollection<PingResult>();

        private const string HostNameOrAddress = "google.com";
        private const int MaxNumberOfDataPoints = 30;
        private const int timeout = 250;

        public Pinger()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
                IsEnabled = true
            };

            PingNow();
            timer.Tick += (sender, args) => PingNow();
        }

        private void PingNow()
        {
            using var pingSender = new Ping();
            PingReply reply = pingSender.Send(HostNameOrAddress, timeout);

            var result = new PingResult
            {
                Timestamp = DateTime.Now,
                Latency = reply.RoundtripTime,
                Success = reply?.Status == IPStatus.Success
            };

            PingResults.Add(result);
            if (PingResults.Count > MaxNumberOfDataPoints)
                PingResults.RemoveAt(0);
        }
    }
}