using Dicom.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.Print
{
    class PrintScuTest
    {
        public static void Test()
        {
            var printJob = new PrintJob("Dicom print job");

            // greyscale
            var greyscaleImg = new DicomImage("D:\\windowing_2.dcm");
            using (var bitmap = greyscaleImg.RenderImage().As<Bitmap>())
            {
                printJob.StartFilmBox("STANDARD\\1,1", "PORTRAIT", "A4");
                printJob.FilmSession.IsColor = false;
                printJob.AddImage(bitmap, 0);
                printJob.EndFilmBox();
            }
        }
    }
}
