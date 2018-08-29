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
    class CStoreSCPProvider : DicomService, IDicomServiceProvider, IDicomCStoreProvider
    {
        public CStoreSCPProvider(Stream stream, Logger log) : base(stream, log) { }

        public DicomCStoreResponse OnCStoreRequest(DicomCStoreRequest request)
        {
            Console.WriteLine("Got request." + request.MessageID);
            return new DicomCStoreResponse(request, DicomStatus.Success);
        }
        public void OnCStoreRequestException(string tempFileName, Exception e)
        {

        }
        public void OnReceiveAssociationRequest(DicomAssociation association)
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
            SendAssociationAccept(association);

        }
        public void OnReceiveAssociationReleaseRequest()
        {
            SendAssociationReleaseResponse();

        }
        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {

        }
        public void OnConnectionClosed(Exception errorCode)
        {

        }
    }
}
