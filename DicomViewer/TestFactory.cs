using DicomViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DicomViewer
{
    class TestFactory
    {
        public static ScenePolyLine CreatePolyline()
        {
            ScenePolyLine line = new ScenePolyLine();
            line.AddPoint(new Point(0, 0));
            line.AddPoint(new Point(100, 100));
            line.AddPoint(new Point(100, 200));
            return line;
        }
    }
}
