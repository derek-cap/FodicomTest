using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DicomViewer.Models
{
    class EmptyCreator : ISceneObjectCreator
    {
        public event EventHandler<SceneObject> SceneObjectCreated;
        public event EventHandler<SceneObject> SceneObjecDrawing;

        public void MouseDown(Point point, bool isLeftButton)
        {
        }

        public void MouseMove(Point point)
        {
        }

        public void MouseUp(Point point, bool isLeftButton)
        { 
        }
    }
}
