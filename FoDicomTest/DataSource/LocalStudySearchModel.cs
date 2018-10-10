using Dicom;
using Dicom.Network;
using FoDicomTest.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.DataSource
{
    class LocalStudySearchModel : IStudySearchModel
    {
        private PacsNode _pacsNode;
        private string LocalAET = "FODICOMSCU";

        private Subject<StudyViewHelper> _studies;

        public IObservable<StudyViewHelper> Studies => _studies as IObservable<StudyViewHelper>;

        public LocalStudySearchModel()
        {
            _studies = new Subject<StudyViewHelper>();
            _pacsNode = PacsNode.Philips;
        }

        public void TestConnection()
        {
            var client = new DicomClient();
            client.AddRequest(new DicomCEchoRequest());
            client.Send(_pacsNode.Host, _pacsNode.Port, false, LocalAET, _pacsNode.AET);
        }

        public async Task SearchStudyAsync()
        {
            var client = new DicomClient();
            client.NegotiateAsyncOps();

            // Find a list of Studies.
            var request = CreateStudyRequest(new QueryConditionModel());

            var studies = new List<DicomDataset>();
            request.OnResponseReceived += (req, response) =>
            {
                DebugStudyResponse(response);
                if (response.Status == DicomStatus.Pending)
                {
                    _studies.OnNext(StudyViewHelper.CreateFrom(response.Dataset));
                }               
            };
            client.AddRequest(request);
            await client.SendAsync(_pacsNode.Host, _pacsNode.Port, false, LocalAET, _pacsNode.AET);
        }

        public IEnumerable<string> SearchSeriesOfStudy(string studyUID)
        {
            var client = new DicomClient();
            client.NegotiateAsyncOps();

            List<string> result = new List<string>();
            var request = CreateSeriesRequestByStudyUID(studyUID);
            request.OnResponseReceived += (req, response) =>
            {
                if (response.Status == DicomStatus.Pending)
                {
                    result.Add(response.Dataset.GetString(DicomTag.SeriesInstanceUID));
                }
            };
            client.AddRequest(request);

            client.Send(_pacsNode.Host, _pacsNode.Port, false, LocalAET, _pacsNode.AET);

            return result;
        }

        //public async Task SearchImageOfSeries(string studyUID, string seriesUID)
        //{
        //    var client = new DicomClient();
        //    client.NegotiateAsyncOps();

        //    var request = DicomCFindRequest.CreateImageQuery(studyUID, seriesUID);

        //    request.OnResponseReceived += (requ, response) =>
        //    {
        //        if (response.Status == DicomStatus.Pending)
        //        {
        //            DicomDataset dataset = response.Dataset;
        //            string imageUID = dataset.Get<string>(DicomTag.SOPInstanceUID);
        //            Console.WriteLine(imageUID);
        //        }
        //        else if (response.Status.State == DicomState.Success)
        //        {
        //            Console.WriteLine("Sending successfully finished");
        //        }
        //        else if (response.Status.State == DicomState.Failure)
        //        {
        //            Console.WriteLine("Error sending datasets: " + response.Status.Description);
        //        }

        //        Console.WriteLine(response.Status);
        //    };

        //    client.AddRequest(request);
        //    await client.SendAsync(_pacsNode.Host, _pacsNode.Port, false, LocalAET, _pacsNode.AET);
        //}

        public async Task SearchImageOfSeries(string studyUID, string seriesUID)
        {
            var client = new DicomClient();
            client.NegotiateAsyncOps();

            var cMoveRequest = CreateCMoveByStudyUID(LocalAET, studyUID, seriesUID);

            cMoveRequest.OnResponseReceived += (DicomCMoveRequest requ, DicomCMoveResponse response) =>
            {
                if (response.Status.State == DicomState.Pending)
                {
                    Console.WriteLine("Sending is in progress. please wait: " + response.Remaining.ToString());
                }
                else if (response.Status.State == DicomState.Success)
                {
                    Console.WriteLine("Sending successfully finished");
                }
                else if (response.Status.State == DicomState.Failure)
                {
                    Console.WriteLine("Error sending datasets: " + response.Status.Description);
                }

                Console.WriteLine(response.Status);
            };

            client.AddRequest(cMoveRequest);
            await client.SendAsync(_pacsNode.Host, _pacsNode.Port, false, LocalAET, _pacsNode.AET);
        }

        //public async Task SearchImageOfSeries(string studyUID, string seriesUID)
        //{
        //    var client = new DicomClient();
        //    client.NegotiateAsyncOps();

        //    var cGetRequest = CreateCGetBySeriesUID(studyUID, seriesUID);
        //    cGetRequest.OnResponseReceived += (req, response) =>
        //    {
        //        Console.WriteLine(response.Status);
        //    };
        //    client.OnCStoreRequest += req =>
        //    {
        //        Console.WriteLine(DateTime.Now.ToString() + " recived");
        //        SaveImage(req.Dataset);
        //        return new DicomCStoreResponse(req, DicomStatus.Success);
        //    };
        //    var pcs = DicomPresentationContext.GetScpRolePresentationContextsFromStorageUids(
        //       DicomStorageCategory.Image,
        //       DicomTransferSyntax.ExplicitVRLittleEndian,
        //       DicomTransferSyntax.ImplicitVRLittleEndian,
        //       DicomTransferSyntax.ImplicitVRBigEndian);
        //    client.AdditionalPresentationContexts.AddRange(pcs);
        //    client.AddRequest(cGetRequest);

        //    await client.SendAsync(_pacsNode.Host, _pacsNode.Port, false, AET, _pacsNode.AET);
        //}


        private DicomCFindRequest CreateStudyRequest(QueryConditionModel condition)
        {
            // there is a built in function to create a Study-level CFind request very easily: 
            // return DicomCFindRequest.CreateStudyQuery(patientName: patientName);

            // but consider to create your own request that contains exactly those DicomTags that
            // you realy need pro process your data and not to cause unneccessary traffic and IO load:
            var request = new DicomCFindRequest(DicomQueryRetrieveLevel.Study);
            // Always add the encoding
            request.Dataset.Add(new DicomTag(0x8, 0x5), "ISO_IR 100");

            // Add the dicom tags with empty values that should be included in the result of QR Server.
            request.Dataset.Add(DicomTag.PatientName, condition.PatientName)
                .Add(DicomTag.PatientID, condition.PatientID)
                .Add(DicomTag.ModalitiesInStudy, condition.Modality)
                .Add(DicomTag.StudyDate, "")
                .Add(DicomTag.StudyInstanceUID, "")
                .Add(DicomTag.StudyDescription, condition.StudyDescription);

            return request;
        }

        public DicomCFindRequest CreateSeriesRequestByStudyUID(string studyInstanceUID)
        {
            // there is a built in function to create a Study-level CFind request very easily: 
            // return DicomCFindRequest.CreateSeriesQuery(studyInstanceUID);

            // but consider to create your own request that contains exactly those DicomTags that
            // you realy need pro process your data and not to cause unneccessary traffic and IO load:
            var request = new DicomCFindRequest(DicomQueryRetrieveLevel.Series);

            request.Dataset.Add(new DicomTag(0x8, 0x5), "ISO_IR 100");

            // add the dicom tags with empty values that should be included in the result
            request.Dataset.Add(DicomTag.SeriesInstanceUID, "");
            request.Dataset.Add(DicomTag.SeriesDescription, "");
            request.Dataset.Add(DicomTag.Modality, "");
            request.Dataset.AddOrUpdate(DicomTag.NumberOfSeriesRelatedInstances, "");

            // add the dicom tags that contain the filter criterias
            request.Dataset.Add(DicomTag.StudyInstanceUID, studyInstanceUID);

            return request;
        }

        public static DicomCGetRequest CreateCGetBySeriesUID(string studyUID, string seriesUID)
        {
            var request = new DicomCGetRequest(studyUID, seriesUID);
            // no more dicomtags have to be set
            return request;
        }

        public static DicomCMoveRequest CreateCMoveBySeriesUID(string destination, string studyUID, string seriesUID)
        {
            var request = new DicomCMoveRequest(destination, studyUID, seriesUID);
            // no more dicomtags have to be set
            return request;
        }

        public static DicomCMoveRequest CreateCMoveByStudyUID(string destination, string studyUID, string seriesUID)
        {
            var request = new DicomCMoveRequest(destination, studyUID, seriesUID);
            // no more dicomtags have to be set
            return request;
        }

        public static void DebugStudyResponse(DicomCFindResponse response)
        {
            if (response.Status == DicomStatus.Pending)
            {
                // print the results
                Console.WriteLine($"Patient {response.Dataset.Get(DicomTag.PatientName, string.Empty)}, {response.Dataset.Get(DicomTag.ModalitiesInStudy, string.Empty)}");
            }
            if (response.Status == DicomStatus.Success)
            {
                Console.WriteLine(response.Status.ToString());
            }

        }

        private static void SaveImage(DicomDataset dataset)
        {
            DirectoryInfo info = new DirectoryInfo("D:\\PacsTemp");
            if (info.Exists == false)
            {
                info.Create();
            }
            string imageUID = dataset.Get<string>(DicomTag.SOPInstanceUID);
            string filename = Path.Combine(info.FullName, imageUID + ".dcm");
            DicomFile file = new DicomFile(dataset);
            file.Save(filename);
        }
    }
}
