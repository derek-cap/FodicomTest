using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTest.Models
{
    class ImageRecord
    {
        [BsonId]
        public string SOPInstanceUID
        {
            get;
            set;
        }                 // 0

        [BsonElement("image_number")]
        public int ImageNumber                       // 1
        {
            get;
            set;
        }

        [BsonElement("dcm_filename")]
        public string DcmFileName                       // 10
        {
            get;
            set;
        }

        [BsonElement("is_archived")]
        public ushort IsArchived                          // 11
        {
            get;
            set;
        }

        [BsonElement("series_uid")]
        public string SeriesInstanceUID                 // 12
        {
            get;
            set;
        }
    }
}
