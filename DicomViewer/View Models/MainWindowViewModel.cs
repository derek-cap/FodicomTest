using DicomViewer.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DicomViewer.View_Models
{
    class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<Scene> SceneCollection { get; } = new ObservableCollection<Scene>();

        private int _rows = 1;
        public int Rows
        {
            get { return _rows; }
            set { SetProperty(ref _rows, value); }
        }

        private int _columns = 1;
        public int Columns
        {
            get { return _columns; }
            set { SetProperty(ref _columns, value); }
        }

        private bool _invertEnabled;
        public bool InvertEnabled
        {
            get { return _invertEnabled; }
            set { SetProperty(ref _invertEnabled, value); }
        }

        private DelegateCommand<object> _smoothCommand;
        public ICommand SmoothCommand => _smoothCommand ?? (_smoothCommand = new DelegateCommand<object>(Smooth));

        private void Smooth(object @value)
        {
            string layout = @value as string;
            switch (layout)
            {
                case "1,1":
                    break;
            }
            if (layout != null && layout.Equals("1,1"))
            {
                MessageBox.Show("Right");
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                var scene = CreateScene(i % 2 == 0);
                SceneCollection.Add(scene);
            }            
        }

        public void FlipXImage()
        {
            SceneCollection.FirstOrDefault()?.FlipXImage();
        }

        public void ApplyWindow(double ww, double wl)
        {
            SceneCollection.FirstOrDefault()?.ApplyWindow(ww, wl);
        }

        internal void ChangeLayout()
        {
            if (Rows == 1)
            {
                Rows = Columns = 2;
            }
            else
            {
                Rows = Columns = 1;
            }
        }

        private Scene CreateScene(bool hasText)
        {
            Scene scene = new Scene();

            var dicom = DicomFactory.CreateDicom();
            var sceneImage = new SceneImage(dicom);
            sceneImage.IsSelected = true;
            scene.AddSceneObject(sceneImage);

            if (hasText)
            {
                var sceneText = new SceneText("DEF", 300, 200);
                scene.AddSceneObject(sceneText);
            }

            scene.AddSceneObject(TestFactory.CreatePolyline());

            return scene;
        }

        public void BestFit(double width, double height)
        {
            double sceneWidth = width / Columns;
            double sceneHeight = height / Rows;

            SceneCollection.AsParallel().ForAll(s => s.BestFit(sceneWidth, sceneHeight));
        }

        public void Rotate(int angle)
        {
            SceneCollection.FirstOrDefault().GetSceneImage()?.Rotate(angle);
        }

        internal void InvertImage()
        {
            SceneCollection.FirstOrDefault().GetSceneImage()?.Invert();
        }

        public void SmoothOrSharpImage(int flag)
        {
            SceneCollection.FirstOrDefault().GetSceneImage()?.SmoothOrSharper(flag);
        }

        internal void ReplaceImage()
        {
            PixelData pd = new PixelData(512, 512, 16);
            pd.Data = new byte[512 * 512 * 2];

            SceneCollection.FirstOrDefault().GetSceneImage()?.UpdateData(pd);
        }

        public void AddPoint()
        {
            var line = SceneCollection.FirstOrDefault().ObjectCollection.OfType<ScenePolyLine>().FirstOrDefault();
            if (line != null)
            {
                Point lastPoint = line.Points.Last();
                lastPoint += new Vector(200, 100);
                line.AddPoint(lastPoint);
            }
        }
    }
}
