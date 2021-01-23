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
using System.Net;

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Interaction logic for EndPointSelector.xaml
    /// </summary>
    public partial class EndPointSelector : UserControl
    {
        public EndPointSelector()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public IPAddress DestinationIp
        {
            get { return (IPAddress)GetValue(DestinationIpProperty); }
            set { SetValue(DestinationIpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestinationIpProperty =
            DependencyProperty.Register("DestinationIp", typeof(IPAddress), typeof(EndPointSelector), new FrameworkPropertyMetadata(IPAddress.Any) { BindsTwoWayByDefault = true });

        public int DestinationPort
        {
            get { return (int)GetValue(DestinationPortProperty); }
            set { SetValue(DestinationPortProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Port.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestinationPortProperty =
            DependencyProperty.Register("DestinationPort", typeof(int), typeof(EndPointSelector), new FrameworkPropertyMetadata(25565) { BindsTwoWayByDefault = true });

        public int LocalPort
        {
            get { return (int)GetValue(LocalPortProperty); }
            set { SetValue(LocalPortProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Port.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalPortProperty =
            DependencyProperty.Register("LocalPort", typeof(int), typeof(EndPointSelector), new FrameworkPropertyMetadata(25565) { BindsTwoWayByDefault = true });

        public List<IPAddress> AvailableIps
        {
            get { return (List<IPAddress>)GetValue(AvailableIpsProperty); }
            set { SetValue(AvailableIpsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableIps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableIpsProperty =
            DependencyProperty.Register("AvailableIps", typeof(List<IPAddress>), typeof(EndPointSelector), new FrameworkPropertyMetadata(Dns.GetHostAddresses(Dns.GetHostName()).ToList()) { BindsTwoWayByDefault = true });

        #endregion
    }
}
