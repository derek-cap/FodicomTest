using Dicom;
using Dicom.Imaging;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FoDicomTest
{
    class Program
    {
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
                string abc = "abc";
                string res = abc + '\0';
                Console.WriteLine(abc.Length);
                Console.WriteLine(res.Length);
                Console.WriteLine(res.ToArray().Length);
                Console.WriteLine(res.Trim('\0').Length);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
