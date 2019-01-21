using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest
{
    interface IPacsNodeReader
    {
        string StoreSCPAE { get; }
        int StoreSCPPort { get; }

        string QRSCPAE { get; }
        int QRSCPPort { get; }

        string ClientAE { get; }


    }
}
