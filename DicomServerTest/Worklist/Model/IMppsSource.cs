using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest.Worklist
{
    public interface IMppsSource
    {
        /// <summary>
        /// The procedure which was previous created with the sopInstanceUID is now completed and some additional 
        /// information is provided.
        /// </summary>
        /// <param name="sopInstanceUID"></param>
        /// <param name="doseDescription"></param>
        /// <param name="affectedInstanceUIDs"></param>
        /// <returns></returns>
        bool SetCompleted(string sopInstanceUID, string doseDescription, List<string> affectedInstanceUIDs);

        /// <summary>
        /// The procedure which was previous created with the sopInstanceUID is now discontinued.
        /// </summary>
        /// <param name="sopInstanceUID"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        bool SetDiscontinued(string sopInstanceUID, string reason);

        /// <summary>
        /// The procedure with given ProcedureStepId is set in progress. The first parameter sopInstanceUID
        /// has to be stored in a database or similar, because the following messages like Discontinue or Completed
        /// do refer to this sopInstanceUID rather than to procedureStepId.
        /// </summary>
        /// <param name="sopInstanceUID"></param>
        /// <param name="procedureStepId"></param>
        /// <returns></returns>
        bool SetInProgress(string sopInstanceUID, string procedureStepId);
    }
}
