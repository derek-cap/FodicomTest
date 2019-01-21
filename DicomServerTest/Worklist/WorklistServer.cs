using Dicom.Network;
using DicomServerTest.Worklist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DicomServerTest.Worklist
{
    class WorklistServer
    {
        private static IDicomServer _server;
        private static Timer _itemsLoaderTimer;

        protected WorklistServer()
        {
        }

        public static string AETitle { get; set; }
        public static IWorklistItemSource CreateItemsSourceService => new WorklistItemsProvider();
        public static List<WorklistItem> CurrentWorklistItems { get; private set; }

        public static void Start(int port, string aet)
        {
            AETitle = aet;
            _server = DicomServer.Create<WorklistService>(port);

            _itemsLoaderTimer = new Timer(state =>
            {
                var newWorklistItems = CreateItemsSourceService.GetAllCurrentWorklistItems();
                CurrentWorklistItems = newWorklistItems;
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        public static void Stop()
        {
            _server?.Dispose();
        }
    }
}
