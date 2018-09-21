using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTest.Models
{
    class StudyRecord
    {
        [BsonId]
        public string StudyUID { get; set; }

        [BsonElement("patient")]
        public PatientRecord Paitent { get; set; }

        [BsonElement("series")]
        public IEnumerable<SeriesRecord> Series { get; set; }
    }
}
