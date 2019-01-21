using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest
{
    class PacsNode
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string AET { get; set; }

        public static PacsNode Philips
        {
            get
            {
                return new PacsNode() { Host = "10.10.21.255", Port = 104, AET = "PHILIPS_DATA" };
            }
        }

        public static PacsNode Strongs
        {
            get
            {
                return new PacsNode() { Host = "10.10.21.87", Port = 105, AET = "CREALIFE" };
            }
        }
    }
}
