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
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Interaction logic for TcpLog.xaml
    /// </summary>
    public partial class TcpLog : UserControl
    {
        public TcpLog()
        {
            InitializeComponent();
        }

        #region Dependency properties

        public ObservableCollection<LogEntry> LogEntries
        {
            get { return (ObservableCollection<LogEntry>)GetValue(LogEntriesProperty); }
            set { SetValue(LogEntriesProperty, value); }
        }
        public static readonly DependencyProperty LogEntriesProperty =
            DependencyProperty.Register("LogEntries", typeof(ObservableCollection<LogEntry>), typeof(TcpLog), new PropertyMetadata(new ObservableCollection<LogEntry>()));

        #endregion

        public void Log(string message)
        {
            LogEntries.Add(new LogEntry(DateTime.Now, message));
        }

        public void Clear(object sender, RoutedEventArgs e)
        {
            LogEntries.Clear();
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Text file (*.txt)|*.txt";
            fileDialog.DefaultExt = ".txt";

            if (fileDialog.ShowDialog() == true)
                foreach (var entry in LogEntries)
                    File.AppendAllText(fileDialog.FileName, entry.FullEntry());
        }
    }

    public class LogEntry : INotifyPropertyChanged
    {
        public LogEntry(DateTime dateTime, string message)
        {
            DateTime = dateTime;
            Message = message;
        }

        public string FullEntry()
        {
            return $"{DateTime} > {Message}\n";
        }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { Set(ref _dateTime, value); }
        }

        private string _message = "";
        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
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
