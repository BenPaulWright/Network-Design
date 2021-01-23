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
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Net_Design_Lib;
using Microsoft.Win32;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Tcp _instance = new Tcp();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _instance.FileReceived += _instance_FileReceived;
            _instance.TcpError += _instance_TcpError;
        }

        private void _instance_TcpError(Exception exception)
        {
            Dispatcher.Invoke(() => { Log.Log(exception.ToString()); });
        }

        private void _instance_FileReceived(byte[] fileData)
        {
            Dispatcher.Invoke(() => { Log.Log($"Received file of length {fileData.Length} bytes"); });

            var result = MessageBox.Show($"Received file of length {fileData.Length} bytes.\nWould you like to save it?", "Client", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    File.WriteAllBytes(saveFileDialog.FileName, fileData);
            }
        }

        private void TransferFile_SendButtonClicked(object sender, string filePath)
        {
            _instance.SendFile(filePath, PacketSize, new System.Net.IPEndPoint(RemoteIp, RemotePort));
            Log.Log($"File Sent: {filePath}");
        }

        private void OpenSocket(object sender, RoutedEventArgs e)
        {
            _instance.EndReceive();
            _instance.BeginReceive(new System.Net.IPEndPoint(Ip, Port));
            Log.Log($"Socket opened on {Ip}");
        }

        #region Bound Properties

        private System.Net.IPAddress _ip = System.Net.IPAddress.Any;
        public System.Net.IPAddress Ip
        {
            get { return _ip; }
            set { Set(ref _ip, value); }
        }

        private int _port = 4321;
        public int Port
        {
            get { return _port; }
            set { Set(ref _port, value); }
        }

        private System.Net.IPAddress _remoteIp = System.Net.IPAddress.Any;
        public System.Net.IPAddress RemoteIp
        {
            get { return _remoteIp; }
            set { Set(ref _remoteIp, value); }
        }

        private int _remotePort = 1234;
        public int RemotePort
        {
            get { return _remotePort; }
            set { Set(ref _remotePort, value); }
        }

        private UInt32 _packetSize = 1024;
        public UInt32 PacketSize
        {
            get { return _packetSize; }
            set { Set(ref _packetSize, value); }
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
