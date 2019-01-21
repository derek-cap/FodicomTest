using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTest
{
    class BsonTest
    {
        public static void Test()
        {
            using (StreamReader sr = File.OpenText("..\\..\\pacs.json"))
            {
                JsonReader jr = new JsonReader(sr);
                BsonDocument elements = BsonSerializer.Deserialize<BsonDocument>(jr);

                Console.WriteLine(elements["SCP"]["CStore"]["Port"].ToInt32());
            }
        }
    }
}
