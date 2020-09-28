using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPFPingMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyPinger myPinger = new MyPinger();
        readonly Random rand = new Random(0);
        public MainWindow()
        {
            InitializeComponent();
            GotNewData(null, null);

            DispatcherTimer newDataTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(.5) };
            newDataTimer.Tick += GotNewData;
            newDataTimer.Start();

        }

        void GotNewData(object sender, EventArgs e)
        {
            // add a new data point to the list
            myPinger.UpdateCollection(new Data { Timestamp = DateTime.Now, Latency = MyPinger.GetPing() });

            // clear the old scatter plot and make a new one
            wpfPlot1.plt.Clear();
            double[] ys = myPinger.Ping1.Select(x => x.Latency).ToArray();
            double[] xs = myPinger.Ping1.Select(x => x.Timestamp.ToOADate()).ToArray();
            wpfPlot1.plt.PlotScatter(xs, ys);
            wpfPlot1.plt.YLabel("Latency in ms");
            wpfPlot1.plt.YLabel("Timestamp");
            wpfPlot1.plt.AxisBounds(Double.NegativeInfinity, Double.PositiveInfinity, 0, 100);
            wpfPlot1.Render();

            // decorate the plot
            wpfPlot1.plt.Title("Web Server Latency");
            wpfPlot1.plt.YLabel("Latency (ms)");
            wpfPlot1.plt.Ticks(dateTimeX: true);

            // since axis limits are being reset on every update, disable mouse pan and zoom
            wpfPlot1.Configure(false, false);
        }
    }
}
