using Dicom;
using Dicom.Imaging;
using Dicom.Imaging.LUT;
using Dicom.Imaging.Render;
using Dicom.IO;
using Dicom.IO.Buffer;
using DicomViewer.Helpers;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DicomViewer.Models
{
    class SceneImage : SceneObject
    {
        private DicomDataset _dicom;
        private DicomImage _originalDicomImage;
        private IImage _image;


        private WriteableBitmap _bitmap;
        public WriteableBitmap Image
        {
            get { return _bitmap; }
            set
            {
                SetProperty(ref _bitmap, value);
            }
        }

        private bool _flipX = false;
        private bool _isInvert = false;
        private int _rotationAngle = 0;
        private int _smoothOrSharpFlag = 0;

        public SceneImage(DicomDataset dicom)
        {
            _dicom = dicom;

            _originalDicomImage = new DicomImage(dicom);
            OriginalWidth = _originalDicomImage.Width;
            OriginalHeight = _originalDicomImage.Height;

            _image = _originalDicomImage.RenderImage();
            _bitmap = _image.AsWriteableBitmap();

            Scale(1.0);
        }

        public void FlipX()
        {
            _flipX = !_flipX;
            _image.Render(0, _flipX, false, _rotationAngle);
            Image = _image.AsWriteableBitmap();
        }

        public void AddOverlay(System.Drawing.Bitmap bitmap, System.Drawing.Color mask)
        {
            var overlayData = DicomOverlayDataFactory.FromBitmap(_dicom, bitmap, mask);
            //DicomFile file = new DicomFile(_dicom);
            //file.Save("D:\\overlay1.dcm");
        }

        public void ApplyWindow(double ww, double wl)
        {
            _originalDicomImage.WindowWidth = ww;
            _originalDicomImage.WindowCenter = wl;
            _image = _originalDicomImage.RenderImage();
            if (_flipX)
            {
                _image.Render(0, _flipX, false, _rotationAngle);
            }
            Image = _image.AsWriteableBitmap();
        }

        public void UpdateData(PixelData pixelData)
        {
            var buffer = new MemoryByteBuffer(pixelData.Data);
            if (pixelData.BPP <= 16)
            {
                var pd = DicomPixelDataFactory.CreateGrayData(pixelData.Width, pixelData.Height, pixelData.BPP);
                pd.AddFrame(buffer);
                _originalDicomImage = new DicomImage(pd.Dataset);
                _image = _originalDicomImage.RenderImage();
                Image = _image.AsWriteableBitmap();
            }
                    
        }

        public void Rotate(int angle)
        {
            _rotationAngle += angle;
            _image.Render(0, _flipX, false, _rotationAngle);
            Image = _image.AsWriteableBitmap();
        }

        public void Invert()
        {
            _isInvert = !_isInvert;
            _originalDicomImage.GrayscaleColorMap = ColorTable.Reverse(_originalDicomImage.GrayscaleColorMap);

            _image = _originalDicomImage.RenderImage();
            Image = _image.AsWriteableBitmap();
        }

        public void SmoothOrSharper(int flag)
        {
            if (_originalDicomImage.GrayscaleColorMap == null) return;
            if (flag == 0) return;

            _smoothOrSharpFlag += flag;

            // Do smooth
            if (flag > 0)
            {
                while (flag-- > 0) DoSmoothOnce();
            }
            else
            {
                while (flag++ < 0) DoSharpOnce();
            }
        }

        private void DoSmoothOnce()
        {
            PixelHelper.Smooth(_originalDicomImage.Width, _originalDicomImage.Height, _image.Pixels);
            _image.Render(0, _flipX, false, _rotationAngle);
            Image = _image.AsWriteableBitmap();
        }

        private void DoSharpOnce()
        {
            PixelHelper.Sharp(_originalDicomImage.Width, _originalDicomImage.Height, _image.Pixels);
            _image.Render(0, _flipX, false, _rotationAngle);
            Image = _image.AsWriteableBitmap();
        }

        public double GetHUValue(double ratioX, double ratioY)
        {
            var imagePoint = TransformToImage(ratioX, ratioY);

            var ipixelData = PixelDataFactory.Create(_originalDicomImage.PixelData, 0);
            return ipixelData.GetPixel((int)imagePoint.X, (int)imagePoint.Y);
        }

        public void DrawGraphics(IEnumerable<IGraphic> grapchics)
        {
            _image.DrawGraphics(grapchics);
        }

        // Anti-clockwise, Right multiply
        private Matrix GetRotationMatrix(int angle)
        {
            double piAngle = (angle % 360) * Math.PI / 180.0;

            double d11 = Math.Cos(piAngle);
            double d12 = Math.Sin(piAngle);
            double d21 = -Math.Sin(piAngle);
            double d22 = Math.Cos(piAngle);

            return new Matrix(d11, d12, d21, d22, 0, 0);
        }

        private Point TransformToImage(double x, double y)
        {
            Point currentPoint = new Point(Image.Width * x, Image.Height * y);

            Point imageCenter = new Point(Image.Width * 0.5, Image.Height * 0.5);
            Point rotPoistion = new Point(currentPoint.X, -currentPoint.Y) + new Vector(-imageCenter.X, imageCenter.Y);

            // Inverse point from UserControl to image. (Inverse rotate image)
            Matrix matrix = GetRotationMatrix(_rotationAngle);
            Point dicomPoint = rotPoistion * matrix;

            var newPoint = dicomPoint - new Vector(-_originalDicomImage.Width * 0.5, _originalDicomImage.Height * 0.5);
            return new Point(newPoint.X, -newPoint.Y);
        }

    }
}
