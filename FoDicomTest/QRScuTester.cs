using Dicom;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest
{
    class QRScuTester
    {
        private static int _port = 105;
        private static string _serverAET = "Minfound.QR";

        private static string _localAET = "FODICOMSCU";

        public static void TestCFind()
        {
            var client = new DicomClient();
            var request = CreateStudyRequestByPatientName("");
            request.OnResponseReceived += (req, response) =>
            {
                Console.WriteLine($"{response.Dataset?.GetSingleValue<string>(DicomTag.PatientName)}");
            };
            client.AddRequest(request);
            client.Send("localhost", _port, false, _serverAET, _localAET);
        }

        private static DicomCFindRequest CreateStudyRequestByPatientName(string patientName)
        {
            var request = new DicomCFindRequest(DicomQueryRetrieveLevel.Study);

            // 
            request.Dataset.AddOrUpdate(DicomTag.SpecificCharacterSet, "ISO_IR 100");

            request.Dataset.AddOrUpdate(DicomTag.PatientName, "");

            request.Dataset.AddOrUpdate(DicomTag.PatientID, "");

            request.Dataset.AddOrUpdate(DicomTag.ModalitiesInStudy, "");

            request.Dataset.AddOrUpdate(DicomTag.StudyDate, "");

            request.Dataset.AddOrUpdate(DicomTag.StudyInstanceUID, "");

            request.Dataset.AddOrUpdate(DicomTag.StudyDescription, "");

            return request;
        }
    }
}
