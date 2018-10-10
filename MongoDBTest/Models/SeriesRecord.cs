using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MongoDBTest.Models
{
    public class SeriesRecord
    {
        [BsonElement("series_uid")]
        public string SeriesUID { get; set; }

        [BsonElement("series_date")]
        public string SeriesDate { get; set; }

        [BsonElement("series_time")]
        public string SeriesTime { get; set; }

        [BsonElement("modality")]
        public string Modality { get; set; }

        [BsonElement("series_number")]
        public string SeriesNumber { get; set; }

        [BsonElement("images")]
        public List<ImageRecord> ImageCollection { get; set; }

        public SeriesRecord()
        {
            ImageCollection = new List<ImageRecord>();
        }
    }
}
