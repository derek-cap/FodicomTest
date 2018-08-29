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
using System.Drawing;
using System.IO;
using DicomViewer.Models;
using DicomViewer.Helpers;

namespace DicomViewer.Views
{
    /// <summary>
    /// ViewerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ViewerControl : UserControl
    {
        internal Scene Scene => DataContext as Scene;

        public ViewerControl()
        {
            InitializeComponent();
        }

        public Bitmap ToBitmap(SceneObject scene)
        {
            var element = FindControl(scene);
            return BitmapHelper.RenderToBmp(element, (int)Math.Ceiling(this.Width), (int)Math.Ceiling(this.Height));
        }

        private FrameworkElement FindControl(SceneObject scene)
        {
            var framework = ScenesControl.ItemContainerGenerator.ContainerFromItem(scene) as ContentPresenter;
            int count = VisualTreeHelper.GetChildrenCount(framework);
            var obj = VisualTreeHelper.GetChild(framework, 0);
            return obj as FrameworkElement;
        }

        private void ItemsControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);

            bool isRightButton = true;
            if (MouseButtonState.Pressed == e.RightButton)
                isRightButton = false;

            Scene?.SceneObjectCreator.MouseDown(point, isRightButton);
        }

        private void ScenesControl_MouseMove(object sender, MouseEventArgs e)
        {
            var imageControl = e.OriginalSource as System.Windows.Controls.Image;
            if (imageControl != null)
            {
                var point = e.GetPosition(imageControl);
                double ratioX = point.X / imageControl.ActualWidth;
                double ratioY = point.Y / imageControl.ActualHeight;

                Scene scene = this.DataContext as Scene;
                if (scene != null)
                {
                    var hu = scene.GetSceneImage().GetHUValue(ratioX, ratioY);
                    HUText.Text = $"{point.X}, {point.Y} = {hu}";
                }
            }
            else
            {
                HUText.Text = "*";
            }
            Scene?.SceneObjectCreator.MouseMove(e.GetPosition(this));
        }

        private void MainGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {         
            Scene?.SceneObjectCreator.MouseUp(e.GetPosition(this), true);
        }
    }
}
