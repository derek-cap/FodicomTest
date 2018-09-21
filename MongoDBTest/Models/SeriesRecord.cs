using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTest.Models
{
    class SeriesRecord
    {
        [BsonElement("series_uid")]
        public string SeriesUID { get; set; }
    }
}
