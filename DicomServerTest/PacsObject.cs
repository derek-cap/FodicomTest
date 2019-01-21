using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest
{
    class PacsObject
    {
        /// <summary>
        /// Pacs Name. Should be unique in current stystem.
        /// </summary>
        public string Name { get; set; }

        public string IPAddress { get; set; }

        public int Port { get; set; }

        public string AE { get; set; }
    }
}
