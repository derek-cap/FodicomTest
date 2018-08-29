using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DicomViewer.Models
{
    class ScenePolyLineCreator : ISceneObjectCreator
    {
        public event EventHandler<SceneObject> SceneObjectCreated;
        public event EventHandler<SceneObject> SceneObjecDrawing;

        private ScenePolyLine _currentPolyLine;

        public void MouseDown(Point point, bool isLeftButton)
        {
            if (_currentPolyLine == null)
            {
                _currentPolyLine = new ScenePolyLine();
            }
            _currentPolyLine.AddPoint(point);
            SceneObjecDrawing?.Invoke(this, _currentPolyLine);

            if (isLeftButton == false)
            {
                SceneObjectCreated?.Invoke(this, _currentPolyLine);
                _currentPolyLine = null;               
            }
        }

        public void MouseMove(Point point)
        {
            _currentPolyLine?.UpdateLastPoint(point);
            SceneObjecDrawing?.Invoke(this, _currentPolyLine);
        }

        public void MouseUp(Point point, bool isLeftButton)
        {
            //_currentPolyLine?.UpdateLastPoint(point);
            //SceneObjectCreated?.Invoke(this, _currentPolyLine);
        }
    }
}
