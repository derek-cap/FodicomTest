using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBTest.Models
{
    public class ImageRecord
    {
        [BsonElement("image_uid")]
        public string SOPInstanceUID { get; set; }

        [BsonElement("image_number")]
        public string ImageNumber { get; set; }

        [BsonElement("referenced_file")]
        public string ReferencedFileID { get; set; }                 
    }
}
