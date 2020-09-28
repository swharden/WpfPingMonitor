using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows.Threading;

namespace WPFPingMonitor
{
    public class MyPinger
    {
        const string HostNameOrAddress = "google.com";
        public ObservableCollection<Data> Ping1 { get; set; } = new ObservableCollection<Data>();

        private const int MaxNumberOfDataPoints = 30;
        public MyPinger()
        {

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2), 
                IsEnabled = true
            };
            timer.Tick += (sender, args) =>
            {
                var d = GetPing();
                UpdateCollection(new Data {Timestamp = DateTime.Now, Latency = d});
            };
            UpdateCollection(new Data { Timestamp = DateTime.Now, Latency = GetPing() });
        }

        public static double GetPing()
        {
            using var pingSender = new Ping();

            long timeout = 250;
            PingReply reply = pingSender.Send(HostNameOrAddress, (int)timeout);
            return reply?.Status == IPStatus.Success ? reply.RoundtripTime : Double.NaN;
        }

        public void UpdateCollection(Data newDatapoint)
        {
            Ping1.Add(newDatapoint);
            if(Ping1.Count > MaxNumberOfDataPoints)
                Ping1.RemoveAt(0);
        }
    }
}