using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts.Wpf;
using LiveCharts;

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Interaction logic for NetworkSpeed.xaml
    /// </summary>
    public partial class NetworkSpeed : UserControl
    {
        public NetworkSpeed()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                Times.Add(DateTime.Now);
                System.Threading.Thread.Sleep(100);
            }
            Data = new LineSeries()
            {
                Title = "NetworkSpeed(Mbps) vs Time",
                Values = new ChartValues<double> { 4, 6, 5, 2, 4 }
            };
        }



        public List<DateTime> Times
        {
            get { return (List<DateTime>)GetValue(TimesProperty); }
            set { SetValue(TimesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Times.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimesProperty =
            DependencyProperty.Register("Times", typeof(List<DateTime>), typeof(NetworkSpeed), new PropertyMetadata(new List<DateTime>()));



        public LineSeries Data
        {
            get { return (LineSeries)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(LineSeries), typeof(NetworkSpeed), new PropertyMetadata(null));


    }
}
