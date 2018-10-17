using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainModel.Models
{
    public class ImageRecord
    {
        [BsonId]
        public string SOPInstanceUID { get; set; }

        [BsonElement("image_number")]
        public string ImageNumber { get; set; }

        [BsonElement("referenced_file")]
        public string ReferencedFileID { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (ImageRecord)obj;
            return string.Equals(SOPInstanceUID, other.SOPInstanceUID) &&
                string.Equals(ImageNumber, other.ImageNumber) &&
                string.Equals(ReferencedFileID, other.ReferencedFileID);
        }

        public override int GetHashCode()
        {
            const int hashIndex = 307;
            var result = SOPInstanceUID != null ? SOPInstanceUID.GetHashCode() : 0;
            result = (result * hashIndex) ^ (ImageNumber != null ? ImageNumber.GetHashCode() : 0);
            result = (result * hashIndex) ^ (ReferencedFileID != null ? ReferencedFileID.GetHashCode() : 0);
            return result;
        }
    }
}
