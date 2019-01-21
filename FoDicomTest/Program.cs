using Dicom;
using Dicom.Imaging;
using Dicom.Imaging.LUT;
using Dicom.Imaging.Render;
using Dicom.IO;
using Dicom.IO.Buffer;
using Dicom.Media;
using Dicom.Network;
using FoDicomTest.Communication;
using FoDicomTest.DataSource;
using FoDicomTest.Fhir;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;

namespace FoDicomTest
{
    class Program
    {


        static void Main(string[] args)
        {
            try
            {
                //StoreScuQueue queue = new StoreScuQueue(SendTask);
                //var client = new DicomClient();
                //string path = @"D:\2.16.840.1.114492.6523652407604437462.167332154024.14114.1";

                //DirectoryInfo info = new DirectoryInfo(path);
                //foreach (var file in info.GetFiles("*.*", SearchOption.AllDirectories))
                //{
                //    DicomCStoreRequest request = new DicomCStoreRequest(file.FullName);
                //    request.OnResponseReceived += (req, rsp) =>
                //    {
                //        Console.WriteLine(rsp.Status + $"{req.MessageID}, {Thread.CurrentThread.ManagedThreadId}");
                //    };
                //    client.AddRequest(request);
                //}
                //client.Send("127.0.0.1", 108, false, "SCU", "FODICOMSCU", 1000);
                //client.Abort();
                //client.Release();

                //var persons = new List<Person>() { new Person(19), new Person(18) };
                //var query = persons.Where(p => p.Name.IndexOf('e') > -1);
                //foreach (var item in query)
                //{
                //    Console.WriteLine(item.Name);
                //}

                //var client = new DicomClient();
                //client.NegotiateAsyncOps();
                //client.AddRequest(new DicomCEchoRequest());
                //client.Send("127.0.0.1", 108, false, "SCU", "FODICOMSCU");

                //StoreScuQueueTest test = new StoreScuQueueTest();
                //test.Run();
                FhirTest.TestJson();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
     
        static async Task SendTask(TransmissionInfo info)
        {
            try
            {
                var client = new DicomClient();

                foreach (var file in info.Folder.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    DicomCStoreRequest request = new DicomCStoreRequest(file.FullName);
                    request.OnResponseReceived += (req, rsp) =>
                    {
           //             Console.WriteLine(rsp.Status + $"{req.MessageID}, {Thread.CurrentThread.ManagedThreadId}");
                    };
                    client.AddRequest(request);
                }

                var task1 = AbortTask(client, info.Token, info.TaskId);

                var node = PacsNode.Strongs;
                var task2 = client.SendAsync(node.Host, node.Port, false, "SCU", node.AET);

                await Task.WhenAny(task1, task2);
                client.Release();
                Console.WriteLine($"Complete {info.TaskId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {info.TaskId} " + ex.Message);
            }
        }

        static async Task AbortTask(DicomClient client, CancellationToken token, int id)
        {
            Console.WriteLine($"Abort running... {id}");
            await Task.Delay(1);
            token.WaitHandle.WaitOne();
            await client.AbortAsync();
            await client.ReleaseAsync();
            Console.WriteLine($"Client abort {id}");
        }

       
    }
}
