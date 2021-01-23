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

namespace NetDesign_UC_Lib
{
    /// <summary>
    /// Interaction logic for FileDrop.xaml
    /// </summary>
    public partial class FileDrop : UserControl
    {
        public FileDrop()
        {
            InitializeComponent();
        }

        private void DropFiles(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filePaths = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();
                FilesDropped?.Invoke(filePaths);
            }
        }

        public delegate void FilesDroppedDelegate(List<string> filePaths);
        public event FilesDroppedDelegate FilesDropped;
    }
}
