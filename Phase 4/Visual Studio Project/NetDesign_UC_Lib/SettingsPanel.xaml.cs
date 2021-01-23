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
    /// Interaction logic for SettingsPanel.xaml
    /// </summary>
    public partial class SettingsPanel : UserControl
    {
        public SettingsPanel()
        {
            InitializeComponent();
        }

        #region Dependecy Properties

        public int MinFileSize
        {
            get { return (int)GetValue(MinFileSizeProperty); }
            set { SetValue(MinFileSizeProperty, value); }
        }
        public static readonly DependencyProperty MinFileSizeProperty =
            DependencyProperty.Register("MinFileSize", typeof(int), typeof(SettingsPanel), new FrameworkPropertyMetadata(0) { BindsTwoWayByDefault = true });

        public int MaxFileSize
        {
            get { return (int)GetValue(MaxFileSizeProperty); }
            set { SetValue(MaxFileSizeProperty, value); }
        }
        public static readonly DependencyProperty MaxFileSizeProperty =
            DependencyProperty.Register("MaxFileSize", typeof(int), typeof(SettingsPanel), new FrameworkPropertyMetadata(1024) { BindsTwoWayByDefault = true });

        public int PacketSize
        {
            get { return (int)GetValue(PacketSizeProperty); }
            set { SetValue(PacketSizeProperty, value); }
        }
        public static readonly DependencyProperty PacketSizeProperty =
            DependencyProperty.Register("PacketSize", typeof(int), typeof(SettingsPanel), new FrameworkPropertyMetadata(1024) { BindsTwoWayByDefault = true });

        #endregion
    }
}
