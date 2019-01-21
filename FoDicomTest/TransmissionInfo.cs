using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoDicomTest
{
    class TransmissionInfo
    {
        public int TaskId { get; set; }
        public DirectoryInfo Folder { get; set; }
        public CancellationToken Token { get; set; }
    }
}
