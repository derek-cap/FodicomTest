using Dicom;
using Dicom.Imaging;
using Dicom.Imaging.LUT;
using Dicom.Imaging.Render;
using Dicom.IO;
using Dicom.IO.Buffer;
using Dicom.Media;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.IO;
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

        private static int[] GetGrayPixelData(DicomDataset dataset)
        {
            DicomPixelData pixelData = DicomPixelData.Create(dataset);
            IByteBuffer buffer = pixelData.GetFrame(0);

            IPixelData data = PixelDataFactory.Create(pixelData, 0);
            var renderOptions = GrayscaleRenderOptions.FromDataset(dataset);

            int[] output = new int[pixelData.Width * pixelData.Height];
            data.Render(new ModalityLUT(renderOptions), output);
            return output;
        }

        static void ToPhilips()
        {
            string path = @"D:\Dicom\for yu qing\Chang_Jian_Rong_GE\SE02";
            var client = new DicomClient();
            foreach (var item in Directory.GetFiles(path))
            {
                DicomCStoreRequest request = new DicomCStoreRequest(item);
                request.OnResponseReceived += (req, rsp) =>
                {
                    Console.WriteLine(rsp.Status);
                };
                client.AddRequest(request);
            }
            PacsNode node = PacsNode.Philips;
            client.Send(node.Host, node.Port, false, "FODICOMSCU", node.AET);
        }

        static void WriteMedia(string path)
        {
            var dicomDirPath = Path.Combine("D:\\", "DICOMDIR.dcm");
            var dirInfo = new DirectoryInfo(path);

            var dicomDir = new DicomDirectory();
            foreach (var file in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var dicomFile = DicomFile.Open(file.FullName);
                dicomDir.AddFile(dicomFile, file.FullName);
            }
            dicomDir.Save(dicomDirPath);
        }

        private static void ReadMedia(string fileName)
        {
            var dicomDirectory = DicomDirectory.Open(fileName);
            foreach (var patientRecord in dicomDirectory.RootDirectoryRecordCollection)
            {
                Console.WriteLine(
                    "Patient: {0} ({1})",
                    patientRecord.GetSingleValue<string>(DicomTag.PatientName),
                    patientRecord.GetSingleValue<string>(DicomTag.PatientID));

                foreach (var studyRecord in patientRecord.LowerLevelDirectoryRecordCollection)
                {
                    Console.WriteLine("\tStudy: {0}", studyRecord.GetSingleValue<string>(DicomTag.StudyInstanceUID));
                    foreach (var seriesRecord in studyRecord.LowerLevelDirectoryRecordCollection)
                    {
                        Console.WriteLine("\t\tSeries: {0}", seriesRecord.GetSingleValue<string>(DicomTag.SeriesInstanceUID));
                        foreach (var imageRecord in seriesRecord.LowerLevelDirectoryRecordCollection)
                        {
                            string sopInstanceUID = imageRecord.GetSingleValue<string>(DicomTag.ReferencedSOPInstanceUIDInFile);
                            string fileId = imageRecord.GetString(DicomTag.ReferencedFileID);
                            Console.WriteLine(
                                "\t\t\tImage: {0} [{1}]",
                                sopInstanceUID,
                                fileId);
                        }
                    }
                }
            }
        }
    }
}
