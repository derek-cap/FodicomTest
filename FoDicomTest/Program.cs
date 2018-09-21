using Dicom;
using Dicom.Imaging;
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
    }
}
