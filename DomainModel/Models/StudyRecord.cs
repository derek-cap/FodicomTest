using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DomainModel.Models
{
    public class StudyRecord
    {
        [BsonId]
        public string StudyUID { get; set; }

        [BsonElement("study_id")]
        public string StudyID { get; set; }

        [BsonElement("study_date")]
        public string StudyDate { get; set; }

        [BsonElement("study_time")]
        public string StudyTime { get; set; }

        [BsonElement("modalities")]
        public string ModalitiesInStudy { get; set; }

        [BsonElement("patient")]
        public PatientRecord Paitent { get; set; }

        [BsonElement("series")]
        public List<SeriesRecord> SeriesCollection { get; set; }

        [BsonElement("study_status")]
        public int StudyStatus { get; set; }

        public StudyRecord()
        {
            SeriesCollection = new List<SeriesRecord>();
        }
    }
}
