using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel = new MainViewModel();

        public event EventHandler Changed;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
            var result = Delay().GetAwaiter().GetResult();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;

            Top = 0;
            Left = 0;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            ProgressBar bar = new ProgressBar();
            
        }

        private async Task<string> Delay()
        {
            await Task.Delay(1000);
            return "Yes";
        }
    }
}
