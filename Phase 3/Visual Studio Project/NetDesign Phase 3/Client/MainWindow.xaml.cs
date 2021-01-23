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
        private Rdt _rdtInstance = new Rdt();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _rdtInstance.FileReceived += _instance_FileReceived;
            _rdtInstance.SendProgressChanged += _rdtInstance_SendProgressChanged;
            _rdtInstance.StatusChanged += _rdtInstance_RdtStatusChanged;
            _rdtInstance.FileTransferComplete += _rdtInstance_FileTransferComplete;
        }

        private void _rdtInstance_FileTransferComplete(int numBytes, TimeSpan timeToCompletion)
        {
            Dispatcher.Invoke(() => { Log.Log($"Sent {numBytes} bytes in {timeToCompletion.TotalMilliseconds}ms"); });
        }

        private void _rdtInstance_SendProgressChanged(double percentComplete)
        {
            Dispatcher.Invoke(() => { PacketSendPercentage = percentComplete; });
        }

        private void _rdtInstance_RdtStatusChanged(string status)
        {
            Dispatcher.Invoke(() => { Log.Log(status); });
        }

        private void _instance_FileReceived(byte[] fileData)
        {
            var result = MessageBox.Show($"Received file of length {fileData.Length} bytes.\nWould you like to save it?", "Client", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    File.WriteAllBytes(saveFileDialog.FileName, fileData);
            }
        }

        private void TransferFile_SendButtonClicked(object sender, List<NetDesign_UC_Lib.FileDataObject> filePaths)
        {
            if (filePaths.Count > 0)
            {
                var filePath = filePaths[0].FilePath;

                _rdtInstance.SendFile(filePath, new System.Net.IPEndPoint(RemoteIp, RemotePort));

                TransferFileUserControl.FileDataObjects.Remove(filePaths[0]);
            }
        }

        private void OpenSocket(object sender, RoutedEventArgs e)
        {
            if ((string)(sender as Button).Content == "Open Socket")
            {
                _rdtInstance.SetCorruptionOptions(DataPacketCorruptionEnabled, DataPacketCorruptionChance, AckPacketCorruptionEnabled, AckPacketCorruptionChance);
                _rdtInstance.BindSocket(new System.Net.IPEndPoint(Ip, Port));
                _rdtInstance.OpenSocket(new System.Net.IPEndPoint(RemoteIp, RemotePort));
                Log.Log($"Socket listener opened on {Ip}:{Port}");
                SocketSelector.IsEnabled = false;
                TargetSelector.IsEnabled = false;
                PacketCorruptor.IsEnabled = false;
                (sender as Button).Content = "Close Socket";
            }
            else
            {
                _rdtInstance.CloseSocket();
                Log.Log($"Socket listener closed on {Ip}:{Port}");
                SocketSelector.IsEnabled = true;
                TargetSelector.IsEnabled = true;
                PacketCorruptor.IsEnabled = true;
                (sender as Button).Content = "Open Socket";
            }
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

        private int _packetSize = 1024;
        public int PacketSize
        {
            get { return _packetSize; }
            set { Set(ref _packetSize, value); }
        }

        private double _packetSendPercentage = 0;
        public double PacketSendPercentage
        {
            get { return _packetSendPercentage; }
            set { Set(ref _packetSendPercentage, value); }
        }

        private double _ackPacketCorruptionChance = 0.05;
        public double AckPacketCorruptionChance
        {
            get { return _ackPacketCorruptionChance; }
            set { Set(ref _ackPacketCorruptionChance, value); }
        }

        private double _dataPacketCorruptionChance = 0.05;
        public double DataPacketCorruptionChance
        {
            get { return _dataPacketCorruptionChance; }
            set { Set(ref _dataPacketCorruptionChance, value); }
        }

        private bool _ackPacketCorruptionEnabled = true;
        public bool AckPacketCorruptionEnabled
        {
            get { return _ackPacketCorruptionEnabled; }
            set { Set(ref _ackPacketCorruptionEnabled, value); }
        }

        private bool _dataPacketCorruptionEnabled = true;
        public bool DataPacketCorruptionEnabled
        {
            get { return _dataPacketCorruptionEnabled; }
            set { Set(ref _dataPacketCorruptionEnabled, value); }
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