using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.IO;

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Interaction logic for FileList.xaml
    /// </summary>
    public partial class FileList : UserControl
    {
        public FileList()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public ObservableCollection<FileDataObject> FileDataObjects
        {
            get { return (ObservableCollection<FileDataObject>)GetValue(FileDataObjectsProperty); }
            set { SetValue(FileDataObjectsProperty, value); }
        }
        public static readonly DependencyProperty FileDataObjectsProperty =
            DependencyProperty.Register("FileDataObjects", typeof(ObservableCollection<FileDataObject>), typeof(FileList), new FrameworkPropertyMetadata(new ObservableCollection<FileDataObject>()) { BindsTwoWayByDefault = true });

        #endregion

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var deleteFiles = new List<FileDataObject>();

                foreach (FileDataObject file in (sender as DataGrid).SelectedItems)
                    deleteFiles.Add(file);

                foreach (var file in deleteFiles)
                    FileDataObjects.Remove(file);
            }
        }
    }
}
