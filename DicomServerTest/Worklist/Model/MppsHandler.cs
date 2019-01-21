using Dicom.Log;
using DicomServerTest.Worklist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest.Worklist
{
    class MppsHandler : IMppsSource
    {
        public static Dictionary<string, WorklistItem> PendingProcedures { get; } = new Dictionary<string, WorklistItem>();
        private readonly Logger _logger;

        public MppsHandler(Logger logger)
        {
            _logger = logger;
        }

        public bool SetCompleted(string sopInstanceUID, string doseDescription, List<string> listOfInstanceUIDs)
        {
            if (!PendingProcedures.ContainsKey(sopInstanceUID)) return false;
            var workItem = PendingProcedures[sopInstanceUID];

            // The MPPS completed message contains some additional informations about the performed procedure.
            // This information are very vendor depending, so read the DICOM Conformance Statement or read
            // the DICOM logfiles to see which informaitons the vendor sends.

            // Since the produre was stopped, we remove it from the list of pending procedures.
            PendingProcedures.Remove(sopInstanceUID);
            return true;
        }

        public bool SetDiscontinued(string sopInstanceUID, string reason)
        {
            if (!PendingProcedures.ContainsKey(sopInstanceUID)) return false;
            var workItem = PendingProcedures[sopInstanceUID];

            // Since the produre was stopped, we remove it from the list of pending procedures.
            PendingProcedures.Remove(sopInstanceUID);
            return true;
        }

        public bool SetInProgress(string sopInstanceUID, string procedureStepId)
        {
            var workItem = WorklistServer.CurrentWorklistItems.FirstOrDefault(w => w.ProcedureStepID == procedureStepId);
            if (workItem == null)
            {
                // The procedureStepId provided cannot be found any more, so the data is invalid or the 
                // modality tries to start a procedure that has been deleted/changed on the RIS side...
                return false;
            }

            // Remember the sopInstanceUID and store the worklistItem to which the sopInstanceUID belongs.
            // You should do this more permanent like in database or in file.
            PendingProcedures.Add(sopInstanceUID, workItem);
            return true;
        }
    }
}
