using Dicom;
using Dicom.Log;
using Dicom.Network;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest
{
    public class QRService : DicomService, IDicomServiceProvider, IDicomCFindProvider, IDicomCEchoProvider,
        IDicomCMoveProvider, IDicomCGetProvider
    {
        private static readonly DicomTransferSyntax[] AcceptedTransferSyntaxes = new DicomTransferSyntax[]
            {
                DicomTransferSyntax.ExplicitVRLittleEndian,
                DicomTransferSyntax.ExplicitVRBigEndian,
                DicomTransferSyntax.ImplicitVRLittleEndian
            };

        private static readonly DicomTransferSyntax[] AcceptedImageTransferSyntaxes = new DicomTransferSyntax[]
            {
                // Lossless
                DicomTransferSyntax.JPEGLSLossless,
                DicomTransferSyntax.JPEG2000Lossless,
                DicomTransferSyntax.JPEGProcess14SV1,
                DicomTransferSyntax.JPEGProcess14,
                DicomTransferSyntax.RLELossless,

                // Lossy
                DicomTransferSyntax.JPEGLSNearLossless,
                DicomTransferSyntax.JPEG2000Lossy,
                DicomTransferSyntax.JPEGProcess1,
                DicomTransferSyntax.JPEGProcess2_4,

                // Uncompressed
                DicomTransferSyntax.ExplicitVRLittleEndian,
                DicomTransferSyntax.ExplicitVRBigEndian,
                DicomTransferSyntax.ImplicitVRLittleEndian
            };

        public string CallingAE { get; protected set; }
        public string CalledAE { get; protected set; }
        public IPAddress RemoteIP { get; private set; }

        public QRService(INetworkStream stream, Encoding fallbackEncoding, Logger log)
            : base(stream, fallbackEncoding, log)
        {
            var pi = stream.GetType().GetProperty("Socket", BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi != null)
            {
                var endPoint = ((Socket)pi.GetValue(stream, null)).RemoteEndPoint as IPEndPoint;
                RemoteIP = endPoint.Address;
            }
            else
            {
                RemoteIP = new IPAddress(new byte[] { 127, 0, 0, 1 });
            }
        }

        public DicomCEchoResponse OnCEchoRequest(DicomCEchoRequest request)
        {
            Console.WriteLine($"Received verification request from AE {CallingAE} with IP: {RemoteIP}");
            return new DicomCEchoResponse(request, DicomStatus.Success);
        }

        public IEnumerable<DicomCFindResponse> OnCFindRequest(DicomCFindRequest request)
        {
            var queryLevel = request.Level;

            var matchingFiles = new List<string>();

            // a QR SCP has to define in a DICOM Conformance Statement for which dicom tags it can query
            // depending on the level of query. Below there only very few parameters evalueated.
            switch (queryLevel)
            {
                case DicomQueryRetrieveLevel.Patient:
                    {
                        var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
                        var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);

                    }
                    break;
                case DicomQueryRetrieveLevel.Study:
                    {
                        var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
                        var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);
                        var accNr = request.Dataset.GetSingleValueOrDefault(DicomTag.AccessionNumber, string.Empty);
                        var studyUID = request.Dataset.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, string.Empty);
                    }
                    break;
                case DicomQueryRetrieveLevel.Series:
                    {

                    }
                    break;
                case DicomQueryRetrieveLevel.Image:
                    yield return new DicomCFindResponse(request, DicomStatus.QueryRetrieveUnableToProcess);
                    yield break;
            }

            // Now read the required dicomtags from the matching files and resturn as results.
            foreach (var matchingFile in matchingFiles)
            {
                var dicomFile = DicomFile.Open(matchingFile);
                var result = new DicomDataset();
                foreach (var requestedTag in request.Dataset)
                {
                    if (dicomFile.Dataset.Contains(requestedTag))
                    {
                        dicomFile.Dataset.CopyTo(result, requestedTag.Tag);
                    }
                    else
                    {
                        result.Add(requestedTag);
                    }
                    yield return new DicomCFindResponse(request, DicomStatus.Pending) { Dataset = result };
                }
            }

            yield return new DicomCFindResponse(request, DicomStatus.Success);
        }

        public IEnumerable<DicomCGetResponse> OnCGetRequest(DicomCGetRequest request)
        {
            List<string> matchingFiles = new List<string>();
            switch (request.Level)
            {
                case DicomQueryRetrieveLevel.Patient:
                    break;
                case DicomQueryRetrieveLevel.Study:
                    break;
                case DicomQueryRetrieveLevel.Series:
                    break;
                case DicomQueryRetrieveLevel.Image:
                    yield return new DicomCGetResponse(request, DicomStatus.QueryRetrieveUnableToPerformSuboperations);
                    yield break;
            }

            foreach (var matchingFile in matchingFiles)
            {
                var storeRequest = new DicomCStoreRequest(matchingFile);
                SendRequestAsync(storeRequest).Wait();
            }
            yield return new DicomCGetResponse(request, DicomStatus.Success);
        }

        public IEnumerable<DicomCMoveResponse> OnCMoveRequest(DicomCMoveRequest request)
        {
            // The c-move request contains the DestinationAE. The data of this AE should be configured somewhere.
            if (request.DestinationAE != "")
            {
                yield return new DicomCMoveResponse(request, DicomStatus.QueryRetrieveMoveDestinationUnknown);
                yield return new DicomCMoveResponse(request, DicomStatus.ProcessingFailure);
                yield break;
            }

            // This data should come from some data storage.
            var destinationPort = 11112;
            var destinationIP = "localhost";

            IDicomImageFinderService finderService = ServiceLocator.DicomImageFinderService;
            List<string> matchingFiles = new List<string>();
            switch (request.Level)
            {
                case DicomQueryRetrieveLevel.Patient:
                    string patientId = request.Dataset.GetSingleValue<string>(DicomTag.PatientID);
                    matchingFiles = finderService.FindFilesByPatient(patientId);
                    break;
                case DicomQueryRetrieveLevel.Study:
                    string studyUID = request.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                    matchingFiles = finderService.FindFilesByStudyUID(studyUID);
                    break;
                case DicomQueryRetrieveLevel.Series:
                    string seriesUID = request.Dataset.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                    matchingFiles = finderService.FindFilesBySeriesUID(seriesUID);
                    break;
                case DicomQueryRetrieveLevel.Image:
                    yield return new DicomCMoveResponse(request, DicomStatus.QueryRetrieveUnableToPerformSuboperations);
                    yield break;
            }
            // Send to c-store.
            DicomClient client = new DicomClient();
            client.NegotiateAsyncOps();
            int storeTotal = matchingFiles.Count;
            int storeDone = 0;
            int storeFailure = 0;
            foreach (var file in matchingFiles)
            {
                var storeRequest = new DicomCStoreRequest(file);
                storeRequest.OnResponseReceived += (req, resp) =>
                {
                    if (resp.Status == DicomStatus.Success)
                    {
                        storeDone++;
                    }
                    else
                    {
                        storeFailure++;
                    }
                    SendResponseAsync(new DicomCMoveResponse(request, DicomStatus.Pending) { Remaining = storeTotal - storeDone - storeFailure, Completed = storeDone }).Wait();
                };
                client.AddRequest(storeRequest);
            }

            var sendTask = client.SendAsync(destinationIP, destinationPort, false, CalledAE, request.DestinationAE);
            sendTask.Wait();

            yield return new DicomCMoveResponse(request, DicomStatus.Success);
        }

        public void OnConnectionClosed(Exception exception)
        {
            Clean();
        }

        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            // Log the abort reason
        }

        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            Clean();
            return SendAssociationReleaseResponseAsync();
        }

        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            CallingAE = association.CallingAE;
            CalledAE = association.CalledAE;

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelFIND
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelMOVE
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelFIND
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelMOVE)
                {
                    pc.AcceptTransferSyntaxes(AcceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelGET
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelGET)
                {
                    pc.AcceptTransferSyntaxes(AcceptedImageTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(AcceptedImageTransferSyntaxes);
                }
                else
                {
                    pc.SetResult(DicomPresentationContextResult.RejectAbstractSyntaxNotSupported);
                }
            }

            return SendAssociationAcceptAsync(association);
        }

        private void Clean()
        {
            // Clean up, lik cancel outstanding move- or get- jobs.
        }
    }
}
