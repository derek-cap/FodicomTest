using Dicom;
using Dicom.Log;
using Dicom.Network;
using DomainModel.Infrastructure;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DicomServerTest
{
    class CStoreSCPProvider : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        private IPacsNodeReader _pacsReader;

        public CStoreSCPProvider(INetworkStream stream, Encoding fallbackEncoding, Logger log)
            : base(stream, fallbackEncoding, log)
        {
            _pacsReader = new PacsNodeReader();
        }

        public DicomCStoreResponse OnCStoreRequest(DicomCStoreRequest request)
        {
            try
            {
                Console.WriteLine("Got request." + request.MessageID + $"  {Thread.CurrentThread.ManagedThreadId}");
                IImporter importer = new Importer(new MongoStudyRepository(), new DicomFileRepository());
                importer.ImportAsync(request.Dataset).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //Thread.Sleep(1000);
            //Console.WriteLine("Do get");
            return new DicomCStoreResponse(request, DicomStatus.Success);
        }

        public void OnCStoreRequestException(string tempFileName, Exception e)
        {

        }

        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            // Assert called AE.
            //if (association.CalledAE != "STORESCP")
            //{
            //    return SendAssociationRejectAsync(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CalledAENotRecognized);
            //}

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                    pc.AcceptTransferSyntaxes(SupportedTransferSyntaxes.AcceptedTransferSyntaxes);
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(SupportedTransferSyntaxes.AcceptedImageTransferSyntaxes);
                }
            }

            Console.WriteLine($"Got association request: {association.CallingAE}, {Thread.CurrentThread.ManagedThreadId}");
            return SendAssociationAcceptAsync(association);

        }

        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            return SendAssociationReleaseResponseAsync();
        }

        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {

        }
        public void OnConnectionClosed(Exception errorCode)
        {

        }

        public DicomCEchoResponse OnCEchoRequest(DicomCEchoRequest request)
        {
            return new DicomCEchoResponse(request, DicomStatus.Success);
        }
    }
}
