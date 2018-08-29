using Dicom.Imaging;
using Dicom.Imaging.LUT;
using Dicom.Imaging.Render;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DicomViewer.Models
{
    class TextGraphic : IGraphic
    {
        private double _fontSize;
        private double _scaleFactor;

        private FormattedText _text;

        public int OriginalWidth { get; }
        public int OriginalHeight { get; }

        public int OriginalOffsetX { get; }

        public int OriginalOffsetY { get; }

        public double ScaleFactor => _scaleFactor;

        public int ScaledWidth => (int)_text.Width;

        public int ScaledHeight => (int)_text.Height;

        public int ScaledOffsetX => (int)(OriginalOffsetX * _scaleFactor);

        public int ScaledOffsetY => (int)(OriginalOffsetY * _scaleFactor);

        public int ZOrder => throw new NotImplementedException();

        public TextGraphic(string text, Point offset, Brush brush, double emSize = 16)
        {
            _fontSize = emSize;
            _scaleFactor = 1;
            OriginalOffsetX = (int)offset.X;
            OriginalOffsetY = (int)offset.Y;

            _text = new FormattedText(text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Calibria"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                emSize,
                brush);

            OriginalWidth = (int)_text.Width;
            OriginalHeight = (int)_text.Height;
        }

        public void BestFit(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void FlipX()
        {
            throw new NotImplementedException();
        }

        public void FlipY()
        {
            throw new NotImplementedException();
        }

        public IImage RenderImage(ILUT lut)
        {                   
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawText(_text, new Point(ScaledOffsetX, ScaledOffsetY));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(ScaledWidth, ScaledHeight, 96, 96, PixelFormats.Bgra32);
            bmp.Render(drawingVisual);

            var overlay = new int[ScaledWidth * ScaledHeight];
            var stride = ScaledWidth * 4;
            bmp.CopyPixels(overlay, stride, 0);

            var image = new WPFImage(ScaledWidth, ScaledHeight);
            image.AsWriteableBitmap().WritePixels(
                new Int32Rect(ScaledOffsetX, ScaledOffsetY, ScaledWidth, ScaledHeight),
                overlay,
                stride,
                0);

            return image;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Rotate(int angle)
        {
            throw new NotImplementedException();
        }

        public void Scale(double scale)
        {
            throw new NotImplementedException();
        }

        public void Transform(double scale, int rotation, bool flipx, bool flipy)
        {
            throw new NotImplementedException();
        }
    }
}
