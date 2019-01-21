using Dicom;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var server = DicomServer.Create<CStoreSCPProvider>(104);
                var server2 = DicomServer.Create<QRService>(105);

                //PacsNodeReader reader = new PacsNodeReader();
                //foreach (var item in reader.PacsNode())
                //{
                //    Console.WriteLine($"{item.Name}, {item.Port}");
                //}

                Console.WriteLine("Running...");
                Console.ReadLine();
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
