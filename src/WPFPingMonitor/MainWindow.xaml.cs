using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace WPFPingMonitor
{
    public partial class MainWindow : Window
    {
        readonly Pinger myPinger = new Pinger();
        private DateTime lastUpdate;

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer plotTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
            plotTimer.Tick += PlotNow;
            plotTimer.Start();
        }

        void PlotNow(object sender, EventArgs e)
        {
            // only update the plot if new data is available
            if (myPinger.PingResults.Count == 0)
                return;
            if (myPinger.PingResults.Last().Timestamp == lastUpdate)
                return;
            lastUpdate = myPinger.PingResults.Last().Timestamp;

            // get the data we wish to plot as double arrays
            PingResult[] successfulPings = myPinger.PingResults.Where(x => x.Success).ToArray();
            double[] xs = successfulPings.Select(x => x.OADate).ToArray();
            double[] ys = successfulPings.Select(x => x.Latency).ToArray();

            // display latencies as a scatter plot
            wpfPlot1.plt.Clear();
            wpfPlot1.plt.PlotScatter(xs, ys);

            // decorate the plot, and disable the mouse since axis limits are set manually
            wpfPlot1.plt.Title("Web Server Latency");
            wpfPlot1.plt.YLabel("Latency (ms)");
            wpfPlot1.plt.Axis(y1: 0, y2: 100);
            wpfPlot1.Configure(enablePanning: false, enableRightClickZoom: false);

            // indicate horizontal values are DateTime and add padding to accomodate large tick labels
            wpfPlot1.plt.Ticks(dateTimeX: true);
            wpfPlot1.plt.Layout(y2LabelWidth: 40);

            // the programmer is in control of when the plot is rendered
            wpfPlot1.Render();
        }
    }
}
