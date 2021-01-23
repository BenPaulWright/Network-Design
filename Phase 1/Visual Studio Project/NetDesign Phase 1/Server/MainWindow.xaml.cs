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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Microsoft.Win32;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            UDP_Server_Static.MessageReceived += (string endPoint, string message) => { MessageLog += $"{endPoint} - {message}\n"; };
            UDP_Server_Static.StatusUpdated += (string message) => { MessageLog += message + "\n"; };
            UDP_Server_Static.FileReceived += FileReceived;
            IpComboBox.ItemsSource = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).ToList();
        }

        private void FileReceived(string endPoint, int numBytes, byte[] fileBuffer, string fileName, string fileExtension)
        {
            IsSaveEnabled = true;
            this.fileName = fileName;
            this.fileExtension = fileExtension;
            fileData = new byte[numBytes];
            Array.Copy(fileBuffer, fileData, numBytes);
            FileName = $"{fileName}{fileExtension}";
            FileSize = numBytes;
        }

        private void OpenSocket(object sender, RoutedEventArgs e)
        {
            UDP_Server_Static.OpenSocket(Ip, Port);
        }

        private void SaveAs(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog() { AddExtension = true, DefaultExt = fileExtension, FileName = fileName, Filter = $"{fileExtension}|*{fileExtension}" };
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, fileData);
        }

        private byte[] fileData;

        private string fileName = "";

        private string fileExtension = "";

        #region Bound Properties

        private System.Net.IPAddress _ip;
        public System.Net.IPAddress Ip
        {
            get { return _ip; }
            set { _ip = value; NotifyPropertyChanged("Ip"); }
        }

        private int _port = 1111;
        public int Port
        {
            get { return _port; }
            set { Set(ref _port, value); }
        }

        private string _messageLog = "";
        public string MessageLog
        {
            get { return _messageLog; }
            set { Set(ref _messageLog, value); }
        }

        private string _FileName = "";
        public string FileName
        {
            get { return _FileName; }
            set { Set(ref _FileName, value); }
        }

        private int _fileSize;
        public int FileSize
        {
            get { return _fileSize; }
            set { Set(ref _fileSize, value); }
        }

        private bool _isSaveEnabled;
        public bool IsSaveEnabled
        {
            get { return _isSaveEnabled; }
            set { Set(ref _isSaveEnabled, value); }
        }

        #endregion

        #region Implements INotifyPropertyChanged
        protected bool Set<t>(ref t field, t value, [CallerMemberName]string propertyName = "")
        {
            if (field == null || EqualityComparer<t>.Default.Equals(field, value)) { return false; }
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
