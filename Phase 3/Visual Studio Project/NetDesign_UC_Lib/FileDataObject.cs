using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.IO;

namespace NetDesign_UC_Lib
{
    public class FileDataObject
    {
        public FileDataObject(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            FilePath = filePath;
            FileName = fileInfo.Name;
            FileSize = fileInfo.Length;
            FileSizeString = GetFileSizeAsString(FileSize);
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern long StrFormatByteSizeW(long qdw, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszBuf, int cchBuf);

        private string GetFileSizeAsString(long fileSize)
        {
            var sb = new StringBuilder(32);
            StrFormatByteSizeW(fileSize, sb, sb.Capacity);
            return sb.ToString();
        }

        #region Bound Properties

        private string _fileName = "";
        public string FileName
        {
            get { return _fileName; }
            private set { Set(ref _fileName, value); }
        }

        private string _filePath = "";
        public string FilePath
        {
            get { return _filePath; }
            private set { Set(ref _filePath, value); }
        }

        private long _fileSize = 0;
        public long FileSize
        {
            get { return _fileSize; }
            private set { Set(ref _fileSize, value); }
        }

        private string _fileSizeString = "";
        public string FileSizeString
        {
            get { return _fileSizeString; }
            private set { Set(ref _fileSizeString, value); }
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
