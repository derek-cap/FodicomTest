using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer.Models
{
    class SceneText : SceneObject
    {
        private string _text = "";
        public string Text
        {
            get { return _text; }
            set
            {
                SetProperty(ref _text, value);
            }
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }

        public SceneText(string text, double x, double y)
        {
            _text = text;
            _x = x;
            _y = y;
        }

        public void MoveTo(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
