using Dicom;
using Dicom.Imaging;
using Dicom.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer.Models
{
    class DicomPixelDataFactory
    {
        public static DicomPixelData CreateGrayData(int width, int height)
        {
            var dataset = new DicomDataset();

            dataset.Add<ushort>(DicomTag.Columns, (ushort)width)

                .Add<ushort>(DicomTag.Rows, (ushort)height)

                .Add<ushort>(DicomTag.BitsAllocated, 8)

                .Add<ushort>(DicomTag.BitsStored, 8)

                .Add<ushort>(DicomTag.HighBit, 7)

                .Add(DicomTag.PixelRepresentation, (ushort)PixelRepresentation.Unsigned)

                .Add(DicomTag.PlanarConfiguration, (ushort)PlanarConfiguration.Interleaved)

                .Add<ushort>(DicomTag.SamplesPerPixel, 1)

                .Add(DicomTag.PhotometricInterpretation, PhotometricInterpretation.Monochrome2.Value);

            var pixelData = DicomPixelData.Create(dataset, true);
            return pixelData;
        }

        public static DicomPixelData CreateGrayData(int width, int height, int bpp = 16)
        {
            var dataset = new DicomDataset();

            dataset.Add<ushort>(DicomTag.Columns, (ushort)width)

                .Add<ushort>(DicomTag.Rows, (ushort)height)

                .Add<ushort>(DicomTag.BitsAllocated, (ushort)bpp)

                .Add<ushort>(DicomTag.BitsStored, (ushort)bpp)

                .Add<ushort>(DicomTag.HighBit, (ushort)(bpp - 1))

                .Add(DicomTag.PixelRepresentation, (ushort)PixelRepresentation.Signed)

                .Add(DicomTag.PlanarConfiguration, (ushort)PlanarConfiguration.Interleaved)

                .Add<ushort>(DicomTag.SamplesPerPixel, 1)

                .Add(DicomTag.PhotometricInterpretation, PhotometricInterpretation.Monochrome2.Value);

            var pixelData = DicomPixelData.Create(dataset, true);
            return pixelData;
        }

        public static DicomPixelData CreateColorData(int width, int height)
        {
            var dataset = new DicomDataset();

            dataset.Add<ushort>(DicomTag.Columns, (ushort)width)

                .Add<ushort>(DicomTag.Rows, (ushort)height)

                .Add<ushort>(DicomTag.BitsAllocated, 8)

                .Add<ushort>(DicomTag.BitsStored, 8)

                .Add<ushort>(DicomTag.HighBit, 7)

                .Add(DicomTag.PixelRepresentation, (ushort)PixelRepresentation.Unsigned)

                .Add(DicomTag.PlanarConfiguration, (ushort)PlanarConfiguration.Interleaved)

                .Add<ushort>(DicomTag.SamplesPerPixel, 3)

                .Add(DicomTag.PhotometricInterpretation, PhotometricInterpretation.Monochrome2.Value);

            var pixelData = DicomPixelData.Create(dataset, true);
            return pixelData;
        }

        public static unsafe PinnedByteArray GetGreyBytes(Bitmap bitmap)
        {
            var pixels = new PinnedByteArray(bitmap.Width * bitmap.Height);

            var data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                bitmap.PixelFormat);

            var srcComponents = bitmap.PixelFormat == PixelFormat.Format24bppRgb ? 3 : 4;
            var dstLine = (byte*)pixels.Pointer;
            var srcLine = (byte*)data.Scan0.ToPointer();

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    var pixel = srcLine + j * srcComponents;
                    int grey = (int)(pixel[0] * 0.3 + pixel[1] * 0.59 + pixel[2] * 0.11);
                    dstLine[j] = (byte)grey;
                }

                srcLine += data.Stride;
                dstLine += data.Width;
            }
            bitmap.UnlockBits(data);

            return pixels;
        }

        private unsafe PinnedByteArray GetColorbytes(Bitmap bitmap)
        {
            var pixels = new PinnedByteArray(bitmap.Width * bitmap.Height * 3);

            var data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                bitmap.PixelFormat);            

            var srcComponents = bitmap.PixelFormat == PixelFormat.Format24bppRgb ? 3 : 4;

            var dstLine = (byte*)pixels.Pointer;
            var srcLine = (byte*)data.Scan0.ToPointer();

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    var srcPixel = srcLine + j * srcComponents;
                    var dstPixel = dstLine + j * 3;
                    //convert from bgr to rgb
                    dstPixel[0] = srcPixel[2];
                    dstPixel[1] = srcPixel[1];
                    dstPixel[2] = srcPixel[0];
                }

                srcLine += data.Stride;
                dstLine += data.Width * 3;
            }

            bitmap.UnlockBits(data);

            return pixels;
        }
    }
}
