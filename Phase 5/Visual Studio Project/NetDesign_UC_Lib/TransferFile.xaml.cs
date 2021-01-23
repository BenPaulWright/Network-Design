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
using System.Collections.ObjectModel;

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

        public ObservableCollection<FileDataObject> FileDataObjects
        {
            get { return (ObservableCollection<FileDataObject>)GetValue(FileDataObjectsProperty); }
            set { SetValue(FileDataObjectsProperty, value); }
        }
        public static readonly DependencyProperty FileDataObjectsProperty =
            DependencyProperty.Register("FileDataObjects", typeof(ObservableCollection<FileDataObject>), typeof(TransferFile), new FrameworkPropertyMetadata(new ObservableCollection<FileDataObject>()) { BindsTwoWayByDefault = true });

        public bool SendButtonEnabled
        {
            get { return (bool)GetValue(SendButtonEnabledProperty); }
            set { SetValue(SendButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty SendButtonEnabledProperty =
            DependencyProperty.Register("SendButtonEnabled", typeof(bool), typeof(TransferFile), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true });

        public double SendPercentage
        {
            get { return (double)GetValue(SendPercentageProperty); }
            set { SetValue(SendPercentageProperty, value); }
        }
        public static readonly DependencyProperty SendPercentageProperty =
            DependencyProperty.Register("SendPercentage", typeof(double), typeof(TransferFile), new FrameworkPropertyMetadata(0.0) { BindsTwoWayByDefault = true });

        #endregion

        private void OnFilesDropped(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
                FileDataObjects.Add(new FileDataObject(filePath));
            SendButtonEnabled = true;
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            SendButtonClicked?.Invoke(this, new List<FileDataObject>(FileDataObjects));
        }

        public delegate void SendButtonClickedDelegate(object sender, List<FileDataObject> filePaths);
        public event SendButtonClickedDelegate SendButtonClicked;
    }
}
