using System;

namespace WPFPingMonitor
{
    public class PingResult
    {
        public double Latency { get; set; }
        public DateTime Timestamp { get; set; }
        public double OADate { get => Timestamp.ToOADate(); }
        public bool Success { get; set; } = false;
    }
}