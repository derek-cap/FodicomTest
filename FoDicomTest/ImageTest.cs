using Dicom;
using Dicom.Imaging;
using Dicom.IO;
using Dicom.IO.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest
{
    class ImageTest
    {
        public static void Run()
        {
            string filename = @"D:\Dicom\1.1.dcm";
            DicomFile file = DicomFile.Open(filename);
            DicomImage image = new DicomImage(file.Dataset);
            //var img = image.RenderImage();
            //PinnedIntArray array = img.Pixels;
            //Console.WriteLine(array.Count/512);
            var pixelData = DicomPixelData.Create(file.Dataset);
            //foreach (var item in typeof(DicomPixelData).GetProperties())
            //{
            //    Console.WriteLine($"{item.Name}, {item.GetValue(pixelData)}");
            //}
            Console.WriteLine($"{image.WindowCenter}, {image.WindowWidth}");
            var img = image.RenderImage();
            img.Render(0, false, false, 90);
           

            Console.WriteLine(pixelData.GetType());
            var newBuffer = CreateBuffer(pixelData);

            var newData = DicomPixelData.Create(file.Dataset, true);
            newData.AddFrame(newBuffer);

            var data2 = DicomPixelData.Create(file.Dataset);
            Console.WriteLine(data2.NumberOfFrames);

            file.Save("D:\\2.dcm");
        }

        private static IByteBuffer CreateBuffer(DicomPixelData originalData)
        {
            var frame = originalData.GetFrame(0);
            
            return new MemoryByteBuffer(frame.Data);
        }
    }
}
