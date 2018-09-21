﻿using Dicom.Network;
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
