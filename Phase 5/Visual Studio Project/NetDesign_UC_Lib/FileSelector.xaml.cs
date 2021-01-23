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
using System.Collections.ObjectModel;

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class FileSelector : UserControl
    {
        public FileSelector()
        {
            InitializeComponent();
        }

        private void OpenFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog() { Multiselect = true };

            if (fileDialog.ShowDialog() == true)
                foreach (var fileName in fileDialog.FileNames)
                    Files.Add(new FileData(fileName, 1024));
        }

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var deleteFiles = new List<FileData>();

                foreach (var file in (sender as DataGrid).SelectedItems)
                    deleteFiles.Add((file as FileData));

                foreach (var file in deleteFiles)
                    Files.Remove(file);
            }
        }

        public ObservableCollection<FileData> Files
        {
            get { return (ObservableCollection<FileData>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }
        public static readonly DependencyProperty FilesProperty =
            DependencyProperty.Register("Files", typeof(ObservableCollection<FileData>), typeof(FileSelector), new PropertyMetadata(new ObservableCollection<FileData>()));
    }

    public class FileData : INotifyPropertyChanged
    {
        public FileData(string filePath, int packetSize)
        {
            FilePath = filePath;
            PacketSize = packetSize;
            RawData = File.ReadAllBytes(FilePath);
            NumBytes = RawData.Length;
        }

        public byte[] RawData { get; set; }

        public List<byte[]> Packets
        {
            get
            {
                var returnPackets = new List<byte[]>();

                for (int i = 0; i < NumPackets; i++)
                {
                    var singlePacket = new byte[PacketSize];

                    Array.Copy(RawData, i * PacketSize, singlePacket, 0, PacketSize);

                    returnPackets.Add(singlePacket);
                }

                return returnPackets;
            }
        }

        private string _filePath = "";
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                Set(ref _filePath, value);
                NotifyPropertyChanged("FileName");
                NotifyPropertyChanged("FileExtension");
            }
        }

        public string FileName
        {
            get => Path.GetFileNameWithoutExtension(FilePath);
        }

        public string FileExtension
        {
            get => Path.GetExtension(FilePath);
        }

        public int NumPackets
        {
            get => (int)Math.Ceiling((decimal)NumBytes / PacketSize);
        }

        private int _numBytes = 0;
        public int NumBytes
        {
            get { return _numBytes; }
            set
            {
                Set(ref _numBytes, value);
                NotifyPropertyChanged("NumPackets");
            }
        }

        private int _packetSize = 1024;
        public int PacketSize
        {
            get { return _packetSize; }
            set
            {
                Set(ref _packetSize, value);
                NotifyPropertyChanged("NumPackets");
            }
        }

        private double _progress = 0;
        public double Progress
        {
            get { return _progress; }
            set { Set(ref _progress, value); }
        }

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
