using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest.Worklist.Model
{
    interface IWorklistItemSource
    {
        /// <summary>
        /// This method queries some source like database or webservice to get a list of all scheduled worklist items.
        /// This method is called periodically.
        /// </summary>
        /// <returns></returns>
        List<WorklistItem> GetAllCurrentWorklistItems();
    }
}
