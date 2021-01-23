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

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Interaction logic for PacketCorruptionOptions.xaml
    /// </summary>
    public partial class PacketCorruptionOptions : UserControl
    {
        public PacketCorruptionOptions()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public double AckPacketCorruptionChance
        {
            get { return (double)GetValue(AckPacketCorruptionChanceProperty); }
            set { SetValue(AckPacketCorruptionChanceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AckPacketCorruptionChance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AckPacketCorruptionChanceProperty =
            DependencyProperty.Register("AckPacketCorruptionChance", typeof(double), typeof(PacketCorruptionOptions), new FrameworkPropertyMetadata(0.5) { BindsTwoWayByDefault = true });

        public double DataPacketCorruptionChance
        {
            get { return (double)GetValue(DataPacketCorruptionChanceProperty); }
            set { SetValue(DataPacketCorruptionChanceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataPacketCorruptionChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataPacketCorruptionChanceProperty =
            DependencyProperty.Register("DataPacketCorruptionChance", typeof(double), typeof(PacketCorruptionOptions), new FrameworkPropertyMetadata(0.5) { BindsTwoWayByDefault = true });

        public bool AckPacketCorruptionEnabled
        {
            get { return (bool)GetValue(AckPacketCorruptionEnabledProperty); }
            set { SetValue(AckPacketCorruptionEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AckPacketCorruptionEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AckPacketCorruptionEnabledProperty =
            DependencyProperty.Register("AckPacketCorruptionEnabled", typeof(bool), typeof(PacketCorruptionOptions), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true });

        public bool DataPacketCorruptionEnabled
        {
            get { return (bool)GetValue(DataPacketCorruptionEnabledProperty); }
            set { SetValue(DataPacketCorruptionEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataPacketCorruptionEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataPacketCorruptionEnabledProperty =
            DependencyProperty.Register("DataPacketCorruptionEnabled", typeof(bool), typeof(PacketCorruptionOptions), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true });

        #endregion
    }
}
