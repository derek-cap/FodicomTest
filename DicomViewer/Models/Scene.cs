using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DicomViewer.Models
{
    class Scene : BindableBase
    {
        private ObservableCollection<SceneObject> _objectCollection = new ObservableCollection<SceneObject>();
        public IEnumerable<SceneObject> ObjectCollection => _objectCollection;

        public ISceneObjectCreator SceneObjectCreator { get; private set; }

        private SceneObject _selectedScene;
        public SceneObject SelectedSceneObject
        {
            get { return _selectedScene; }
            set { SetProperty(ref _selectedScene, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }

        private DelegateCommand _selectCommand;
        public ICommand SelectCommand
        {
            get { return _selectCommand ?? (_selectCommand = new DelegateCommand(() => IsSelected = !IsSelected)); }
        }

        public Scene()
        {
            ChangeCreator();            
        }

        public void AddSceneObject(SceneObject scene)
        {
            if (!_objectCollection.Contains(scene))
            {
                _objectCollection.Add(scene);
            }
        }

        public void FlipXImage()
        {
            var image = _objectCollection.FirstOrDefault(s => s is SceneImage);
            if (image != null)
            {
                (image as SceneImage).FlipX();
            }
        }

        internal void MoveTo(double v1, double v2)
        {
            (SelectedSceneObject as SceneText)?.MoveTo(v1, v2);
        }

        public void AddOverlay(Bitmap bitmap, Color mask)
        {
            var image = _objectCollection.FirstOrDefault(s => s is SceneImage);
            if (image != null)
            {
                (image as SceneImage).AddOverlay(bitmap, mask);
            }
        }

        public void ApplyWindow(double ww, double wl)
        {
            var image = _objectCollection.FirstOrDefault(s => s is SceneImage);
            if (image != null)
            {
                (image as SceneImage).ApplyWindow(ww, wl);
            }
        }

        public SceneImage GetSceneImage()
        {
            return _objectCollection.FirstOrDefault(s => s is SceneImage) as SceneImage; 
        }

        internal void BestFit(double sceneWidth, double sceneHeight)
        {
            var image = GetSceneImage();
            image?.BestFit(sceneWidth, sceneHeight);
        }

        public void ChangeCreator()
        {
            SceneObjectCreator = new ScenePolyLineCreator();
            SceneObjectCreator.SceneObjectCreated += OnSceneObjectCreated;
            SceneObjectCreator.SceneObjecDrawing += OnSceneObjectDrawing;
        }

        public void OnSceneObjectCreated(object sender, SceneObject sceneObject)
        {
            
        }

        public void OnSceneObjectDrawing(object sender, SceneObject sceneObject)
        {
            if (_objectCollection.Contains(sceneObject) == false)
            {
                _objectCollection.Add(sceneObject);
            }
        }
    }
}
