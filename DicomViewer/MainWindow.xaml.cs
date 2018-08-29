using DicomViewer.Models;
using DicomViewer.View_Models;
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

namespace DicomViewer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel = new MainWindowViewModel();

        public MainWindow()
        {           
            InitializeComponent();
            _viewModel.Initialize();
            this.DataContext = _viewModel;
        }

        private void FlipXButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FlipXImage();
        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Rotate(-90);
        }

        private void LayoutButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeLayout();
        }

        private void AddPointButton_Click(object sender, RoutedEventArgs e)
        {
            //var sceneText = _viewModel.DicomView.SceneCollection.FirstOrDefault(s => s is SceneText);

            //var bitmap = ViewControl.ToBitmap(sceneText);
            //bitmap.Save("D:\\1.bmp");
            //_viewModel.DicomView.AddOverlay(bitmap, System.Drawing.Color.Green);
            _viewModel.AddPoint();
        }

        private void Windows_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ApplyWindow(300, 100);
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ReplaceImage();
        }

        private void Invert_Click(object sender, RoutedEventArgs e)
        {
            //         _viewModel.InvertImage();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid grid = sender as Grid;
            _viewModel.BestFit(grid.ActualWidth, grid.ActualHeight);
        }


        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //var point = e.GetPosition(Image);
            //MessageBox.Show($"{point.X},{point.Y}");
        }

        private void SmoothButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SmoothOrSharpImage(-1);
        }
    }
}
