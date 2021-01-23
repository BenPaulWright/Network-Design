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
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : UserControl
    {
        public IconButton()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public MaterialDesignThemes.Wpf.PackIconKind Kind
        {
            get { return (MaterialDesignThemes.Wpf.PackIconKind)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }
        public static readonly DependencyProperty KindProperty =
            DependencyProperty.Register("Kind", typeof(MaterialDesignThemes.Wpf.PackIconKind), typeof(IconButton), new FrameworkPropertyMetadata(MaterialDesignThemes.Wpf.PackIconKind.Abc) { BindsTwoWayByDefault = true });

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(sender, e);
        }

        public delegate void ClickDelegate(object sender, RoutedEventArgs e);
        public event ClickDelegate Click;
    }
}
