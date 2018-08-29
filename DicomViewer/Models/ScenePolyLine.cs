using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DicomViewer.Models
{
    class ScenePolyLine : SceneObject
    {
        private PointCollection _points = new PointCollection();
        public PointCollection Points
        {
            get { return _points; }
            private set
            {
                SetProperty(ref _points, value);
            }
        }

        public void AddPoint(Point point)
        {
            _points.Add(point);
            RefreshGraphic();
        }

        public void UpdateLastPoint(Point point)
        {
            if (_points.Count > 1)
            {
                _points.Remove(_points.LastOrDefault());
            }
            AddPoint(point);
        }

        private void RefreshGraphic()
        {
            Points = new PointCollection(_points);
        }
    }
}
