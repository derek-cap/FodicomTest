using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DicomViewer.Helpers
{
    class BitmapHelper
    {
        public static Bitmap RenderToBmp(FrameworkElement element, int width, int height)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(element);
            BmpBitmapEncoder encode = new BmpBitmapEncoder();
            encode.Frames.Add(BitmapFrame.Create(bitmap));

            var stream = new MemoryStream();
            encode.Save(stream);

            return new Bitmap(stream);
        }
    }
}
