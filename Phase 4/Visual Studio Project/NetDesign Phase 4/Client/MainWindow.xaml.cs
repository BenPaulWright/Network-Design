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

        private void _rdtInstance_FileTransferComplete(long numBytes, TimeSpan timeToCompletion)
        {
            Dispatcher.Invoke(() =>
            {
                Log.Log($"Sent {numBytes} bytes in {timeToCompletion.TotalMilliseconds}ms");
                TransferFileUserControl.IsEnabled = true;
            });
        }

        private void _rdtInstance_SendProgressChanged(double percentComplete)
        {
            Dispatcher.Invoke(() => { PacketSendPercentage = percentComplete; });
        }

        private void _rdtInstance_RdtStatusChanged(string status)
        {
            Dispatcher.Invoke(() => { Log.Log(status); });
        }

        private void _instance_FileReceived(FileInfo tempFileInfo, string fileName)
        {
            //var result = MessageBox.Show($"Received: {fileName}\nSize: {tempFileInfo.Length} bytes\nWould you like to save it?", "Client", MessageBoxButton.YesNo);
            //if (result == MessageBoxResult.Yes)
            //{
            //    var saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.Filter = $"(*.{System.IO.Path.GetExtension(fileName)})|*.{System.IO.Path.GetExtension(fileName)}";
            //    saveFileDialog.DefaultExt = System.IO.Path.GetExtension(fileName);
            //    saveFileDialog.AddExtension = true;
            //    if (saveFileDialog.ShowDialog() == true)
            //        File.Copy(tempFileInfo.FullName, saveFileDialog.FileName, true);
            //}
            File.Delete(tempFileInfo.FullName);
        }

        private void TransferFile_SendButtonClicked(object sender, List<NetDesign_UC_Lib.FileDataObject> filePaths)
        {
            List<string> filePathStrings = new List<string>();
            foreach (NetDesign_UC_Lib.FileDataObject fileObj in filePaths)
                filePathStrings.Add(fileObj.FilePath);

            _rdtInstance.SendFilesAsync(filePathStrings.ToArray(), new System.Net.IPEndPoint(RemoteIp, RemotePort));

            //if (filePaths.Count > 0)
            //{
            //    TransferFileUserControl.IsEnabled = false;

            //    var filePath = filePaths[0].FilePath;

            //    _rdtInstance.SendFile(filePath, new System.Net.IPEndPoint(RemoteIp, RemotePort));

            //    TransferFileUserControl.FileDataObjects.Remove(filePaths[0]);
            //}
        }

        private void OpenSocket(object sender, RoutedEventArgs e)
        {
            _rdtInstance.SetCorruptionOptions(PacketErrorInfo);
            _rdtInstance.BeginReceive((ushort)Port);

            string logString = $"Socket opened on {Port}";
            if (PacketErrorInfo.DataCorruptionEnabled)
                logString += $"\nDC: {PacketErrorInfo.DataCorruptionChance * 100}%";
            if (PacketErrorInfo.AckCorruptionEnabled)
                logString += $"\nAC: {PacketErrorInfo.AckCorruptionChance * 100}%";
            if (PacketErrorInfo.DataDropEnabled)
                logString += $"\nDD: {PacketErrorInfo.DataDropChance * 100}%";
            if (PacketErrorInfo.AckDropEnabled)
                logString += $"\nAD: {PacketErrorInfo.AckDropChance * 100}%";

            Log.Log(logString);
        }

        #region Bound Properties

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

        private NetDesign_UC_Lib.PacketErrorInfo _packetErroInfo = new NetDesign_UC_Lib.PacketErrorInfo();
        public NetDesign_UC_Lib.PacketErrorInfo PacketErrorInfo
        {
            get { return _packetErroInfo; }
            set { Set(ref _packetErroInfo, value); }
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

        private void Window_Closed(object sender, EventArgs e)
        {
            _rdtInstance.Dispose();
        }
    }
}