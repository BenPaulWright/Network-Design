using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Holds the packt corruption and drop options
    /// </summary>
    public class PacketErrorInfo : INotifyPropertyChanged
    {
        // Ack Corruption
        private bool _ackCorruptionEnabled = false;
        public bool AckCorruptionEnabled
        {
            get { return _ackCorruptionEnabled; }
            set { Set(ref _ackCorruptionEnabled, value); }
        }

        private double _ackCorruptionChance = 0.01;
        public double AckCorruptionChance
        {
            get { return _ackCorruptionChance; }
            set { Set(ref _ackCorruptionChance, value); }
        }

        // Data Corruption
        private bool _dataCorruptionEnabled = false;
        public bool DataCorruptionEnabled
        {
            get { return _dataCorruptionEnabled; }
            set { Set(ref _dataCorruptionEnabled, value); }
        }

        private double _dataCorruptionChance = 0.01;
        public double DataCorruptionChance
        {
            get { return _dataCorruptionChance; }
            set { Set(ref _dataCorruptionChance, value); }
        }

        // Ack Drop
        private bool _ackDropEnabled = false;
        public bool AckDropEnabled
        {
            get { return _ackDropEnabled; }
            set { Set(ref _ackDropEnabled, value); }
        }

        private double _ackDropChance = 0.01;
        public double AckDropChance
        {
            get { return _ackDropChance; }
            set { Set(ref _ackDropChance, value); }
        }

        // Data Drop
        private bool _dataDropEnabled = false;
        public bool DataDropEnabled
        {
            get { return _dataDropEnabled; }
            set { Set(ref _dataDropEnabled, value); }
        }

        private double _dataDropChance = 0.01;
        public double DataDropChance
        {
            get { return _dataDropChance; }
            set { Set(ref _dataDropChance, value); }
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
