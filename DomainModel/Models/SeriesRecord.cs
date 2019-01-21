using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DomainModel.Models
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

        [BsonElement("series_description")]
        public string SeriesDescription { get; set; }

        [BsonElement("images")]
        private List<ImageRecord> _images;

        public IReadOnlyCollection<ImageRecord> ImageCollection => _images;

        public SeriesRecord()
        {
            _images = new List<ImageRecord>();
        }

        public void AddImage(ImageRecord image)
        {
            _images.Add(image);
        }
    }
}
