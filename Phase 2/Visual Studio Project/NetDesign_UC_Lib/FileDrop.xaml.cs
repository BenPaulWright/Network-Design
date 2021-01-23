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

        #region Dependency Properties

        public List<string> ValidExtensions
        {
            get { return (List<string>)GetValue(ValidExtensionsProperty); }
            set { SetValue(ValidExtensionsProperty, value); }
        }
        public static readonly DependencyProperty ValidExtensionsProperty =
            DependencyProperty.Register("ValidExtensions", typeof(List<string>), typeof(FileDrop), new PropertyMetadata(new List<string>() { ".bmp" }));

        public SolidColorBrush FileValidBrush
        {
            get { return (SolidColorBrush)GetValue(FileValidBrushProperty); }
            set { SetValue(FileValidBrushProperty, value); Console.WriteLine("Color Set"); }
        }
        public static readonly DependencyProperty FileValidBrushProperty =
            DependencyProperty.Register("FileValidBrush", typeof(SolidColorBrush), typeof(FileDrop), new PropertyMetadata(Brushes.Black));

        #endregion

        private void DropFile(object sender, DragEventArgs e)
        {
            string[] filePaths = { "" };

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (IsFileValid(filePaths[0]))
                FileDropped?.Invoke(filePaths[0]);

            FileValidBrush = Brushes.Black;
        }

        private void FileDragEnter(object sender, DragEventArgs e)
        {
            string[] filePaths = { "" };

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            FileValidBrush = IsFileValid(filePaths[0]) ? Brushes.LimeGreen : Brushes.Red;
        }

        private void FileDragLeave(object sender, DragEventArgs e)
        {
            FileValidBrush = Brushes.Black;
        }

        private bool IsFileValid(string filePath)
        {
            return ValidExtensions.Contains(System.IO.Path.GetExtension(filePath));
        }

        public delegate void FileDroppedDelegate(string filePath);
        public event FileDroppedDelegate FileDropped;

        private void Root_DragLeave(object sender, DragEventArgs e)
        {

        }
    }
}
