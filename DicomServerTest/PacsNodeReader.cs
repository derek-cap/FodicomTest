using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest
{
    class PacsNodeReader : IPacsNodeReader
    {
        private const string CONFIGFILE = "pacs.json";

        private string _storeScpAE;
        private int _storeScpPort;

        private string _qrScpAE;
        private int _qrScpPort;

        public string StoreSCPAE => _storeScpAE;

        public int StoreSCPPort => _storeScpPort;

        public string QRSCPAE => _qrScpAE;

        public int QRSCPPort => _qrScpPort;

        public string ClientAE { get; }

        public PacsNodeReader()
        {
            using (StreamReader sr = new StreamReader(CONFIGFILE))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JObject o = JObject.Load(reader);
                var scp = o["SCP"];

                _storeScpAE = (string)scp["CStore"]["AE"];
                _storeScpPort = (int)scp["CStore"]["Port"];

                _qrScpAE = (string)scp["QR"]["AE"];
                _qrScpPort = (int)scp["QR"]["Port"];

                var scu = o["SCU"];
                ClientAE = (string)scu["CallingAE"];
            }
        }

        public IEnumerable<PacsObject> PacsNode()
        {
            using (StreamReader sr = new StreamReader(CONFIGFILE))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JObject o = JObject.Load(reader);
                var scu = o["SCU"]["PACS"];

                List<PacsObject> pacs = JsonConvert.DeserializeObject<List<PacsObject>>(scu.ToString());

                //var query = from s in scu
                //            select new
                //            {
                //                Name = s["Name"],
                //                CalledAE = s[""],
                //                Port = s["Port"],
                //                IP = s["IP"]
                //            };

                return pacs;
            }
        }

        public void AddOrUpdate(PacsObject one)
        {
            using (StreamReader sr = new StreamReader(CONFIGFILE))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JObject o = JObject.Load(reader);

            }

            using (StreamWriter sw = new StreamWriter(CONFIGFILE))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                
            }
        }
    }
}
