using MongoDB.Bson.Serialization.Attributes;

namespace DomainModel.Models
{
    public class PatientRecord
    {
        [BsonElement("patient_name")]
        public string PatientName { get; set; }

        [BsonElement("patient_id")]
        public string PatientID { get; set; }

        [BsonElement("patient_birthdate")]
        public string PatientBirthDate { get; set; }

        [BsonElement("patient_sex")]
        public string PatientSex { get; set; }
    }
}
