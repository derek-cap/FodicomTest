using Dicom;
using Dicom.Log;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest
{
    class CStoreSCPProvider : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        public CStoreSCPProvider(INetworkStream stream, Encoding fallbackEncoding, Logger log)
            : base(stream, fallbackEncoding, log) { }

        public DicomCStoreResponse OnCStoreRequest(DicomCStoreRequest request)
        {
            Console.WriteLine("Got request." + request.MessageID);
            return new DicomCStoreResponse(request, DicomStatus.Success);
        }

        public void OnCStoreRequestException(string tempFileName, Exception e)
        {

        }

        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                    pc.SetResult(DicomPresentationContextResult.Accept);
                else
                {
                    //pc.SetResult(DicomPresentationContextResult.RejectAbstractSyntaxNotSupported);  
                }
                if (pc.AbstractSyntax == DicomUID.CTImageStorage)
                {
                    pc.SetResult(DicomPresentationContextResult.Accept);
                }
            }

            Console.WriteLine("Got message");
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
