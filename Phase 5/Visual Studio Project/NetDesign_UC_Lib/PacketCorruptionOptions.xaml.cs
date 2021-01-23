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

        public PacketErrorInfo packetErrorInfo
        {
            get { return (PacketErrorInfo)GetValue(packetErrorInfoProperty); }
            set { SetValue(packetErrorInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for packetErrorInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty packetErrorInfoProperty =
            DependencyProperty.Register("packetErrorInfo", typeof(PacketErrorInfo), typeof(PacketCorruptionOptions), new PropertyMetadata(new PacketErrorInfo()));

        #endregion
    }
}
