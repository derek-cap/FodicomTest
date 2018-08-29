using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DicomViewer.Models
{
    interface ISceneObjectCreator
    {
        event EventHandler<SceneObject> SceneObjectCreated;
        event EventHandler<SceneObject> SceneObjecDrawing;

        void MouseDown(Point point, bool isLeftButton);
        void MouseMove(Point point);
        void MouseUp(Point point, bool isLeftButton);
    }
}
