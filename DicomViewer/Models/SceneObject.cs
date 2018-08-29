using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer.Models
{
    public class SceneObject : BindableBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public double OriginalOffsetX { get; set; }
        public double OriginalOffsetY { get; set; }

        public double OriginalWidth { get; set; }
        public double OriginalHeight { get; set; }

        private double _scaleFactor;
        public double ScaleFactor => _scaleFactor;

        public double ScaledOffsetX => OriginalOffsetX * _scaleFactor;
        public double ScaledOffsetY => OriginalOffsetY * _scaleFactor;

        public double ScaledWidth => OriginalWidth * _scaleFactor;
        public double ScaledHeight => OriginalHeight * _scaleFactor;

        private double _offsetX;
        public double OffsetX
        {
            get { return _offsetX; }
            set { SetProperty(ref _offsetX, value); }
        }

        private double _offsetY;
        public double OffsetY
        {
            get { return _offsetY; }
            set { SetProperty(ref _offsetY, value); }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        public void Scale(double scale)
        {
            if (Math.Abs(scale - _scaleFactor) <= double.Epsilon) return;

            _scaleFactor = scale;
            UpdatePosition();
        }

        public void BestFit(double width, double height)
        {
            double xF = width / OriginalWidth;
            double yF = height / OriginalHeight;
            Scale(Math.Min(xF, yF));
        }

        protected void UpdatePosition()
        {
            OffsetX = ScaledOffsetX;
            OffsetY = ScaledOffsetY;
            Width = ScaledWidth;
            Height = ScaledHeight;
        }
    }
}
