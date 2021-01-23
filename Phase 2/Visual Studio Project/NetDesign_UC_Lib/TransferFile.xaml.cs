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
    /// Interaction logic for TransferFile.xaml
    /// </summary>
    public partial class TransferFile : UserControl
    {
        public TransferFile()
        {
            InitializeComponent();
        }

        #region Dependecy Properties

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(TransferFile), new FrameworkPropertyMetadata("") { BindsTwoWayByDefault = true });

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(TransferFile), new FrameworkPropertyMetadata("") { BindsTwoWayByDefault = true });

        public bool SendButtonEnabled
        {
            get { return (bool)GetValue(SendButtonEnabledProperty); }
            set { SetValue(SendButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty SendButtonEnabledProperty =
            DependencyProperty.Register("SendButtonEnabled", typeof(bool), typeof(TransferFile), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true });

        #endregion

        private void OnFileDropped(string filePath)
        {
            FilePath = filePath;
            FileName = System.IO.Path.GetFileName(filePath);
            SendButtonEnabled = true;
        }

        private void Send(object sender, MouseButtonEventArgs e)
        {
            SendButtonClicked?.Invoke(this, FilePath);
        }

        public delegate void SendButtonClickedDelegate(object sender, string filePath);
        public event SendButtonClickedDelegate SendButtonClicked;
    }
}
