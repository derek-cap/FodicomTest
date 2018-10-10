using Dicom;
using Dicom.Imaging;
using Dicom.Media;
using Dicom.Network;
using FoDicomTest.Communication;
using FoDicomTest.DataSource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FoDicomTest
{
    class Program
    {
        class Person
        {
            public string Name { get; set; }
            public int Aget { get; set; }
        }

        static void Main(string[] args)
        {
            try
            {
                //var client = new DicomClient();
                //for (int i = 0; i < 10; i++)
                //{
                //    DicomCStoreRequest request = new DicomCStoreRequest("D:\\123.dcm");
                //    request.OnResponseReceived += (req, rsp) =>
                //    {
                //        Console.WriteLine(rsp.Status);
                //    };
                //    client.AddRequest(request);

                //    DicomCFindRequest request2 = DicomCFindRequest.CreateWorklistQuery();


                //}
                //client.Send("127.0.0.1", 12345, false, "SCU", "ANY-SCP");

                //   string path = @"D:\Dicom\Chen^Bi^Sheng\SE01";
                //   var files = new DirectoryInfo(path).GetFiles();
                //   foreach (var item in files)
                //   {
                //       FileStream stream = File.Open(item.FullName, FileMode.Open);
                //       //DicomFile file = DicomFile.Open(item.FullName);
                //       DicomFile file = DicomFile.Open(stream);
                ////       stream.Close();
                //       string uid = file.Dataset.Get<string>(DicomTag.StudyInstanceUID);
                //       DicomImage image = new DicomImage(file.Dataset);
                //       Console.WriteLine(uid.Length);
                //       //string newName = item.FullName + "_fo";
                //       //file.Save(newName);
                //       file.Save(item.FullName);
                //   }
                //   ImageTest.Run();


                //SearchModelTest test = new SearchModelTest();
                //test.Test().Wait();

                string path = @"C:\DATAPART2\FMIDICMFiles\2.16.840.1.114492.4530665600623454105.206247562715.45460.1";
           //     WriteMedia(path);
                ReadMedia("D:\\DICOMDIR.dcm");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
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
