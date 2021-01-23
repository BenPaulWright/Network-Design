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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Microsoft.Win32;

namespace Client
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
            UDP_Client_Static.MessageReceived += MessageReceived;
            IpComboBox.ItemsSource = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).ToList();
        }

        private void MessageReceived(string endPoint, string message)
        {
            MessageLog += $"{endPoint} - {message}\n";
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            UDP_Client_Static.Send(Ip, Port, Message);
        }

        private void SendFile(object sender, RoutedEventArgs e)
        {
            UDP_Client_Static.SendFile(Ip, Port, filePath);
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                FileName = Path.GetFileName(filePath);
                FileSize = File.ReadAllBytes(filePath).Length;
                IsSendFileEnabled = FileSize <= 131072;
            }
        }

        private string filePath = "";

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

        private string _message = "Hello World!";
        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
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

        private bool _isSendFileEnabled;
        public bool IsSendFileEnabled
        {
            get { return _isSendFileEnabled; }
            set { Set(ref _isSendFileEnabled, value); }
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
