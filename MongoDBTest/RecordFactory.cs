using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTest
{
    class RecordFactory
    {
        public static IEnumerable<ImageRecord> CreateImageRecords()
        {
            yield return new ImageRecord() { SOPInstanceUID = "s1", ImageNumber = "1" };
            yield return new ImageRecord() { SOPInstanceUID = "s2", ImageNumber = "2" };
            yield return new ImageRecord() { SOPInstanceUID = "s3", ImageNumber = "3" };
        }
    }
}
