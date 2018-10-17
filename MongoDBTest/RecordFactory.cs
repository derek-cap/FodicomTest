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

        public static IEnumerable<StudyRecord> CreateStudies()
        {
            yield return new StudyRecord() { StudyUID = "1", Paitent = new PatientRecord(), SeriesCollection = new List<SeriesRecord>() { new SeriesRecord() { SeriesUID = "1" } } };
            yield return new StudyRecord()
            { StudyUID = "2", Paitent = new PatientRecord(), SeriesCollection = new List<SeriesRecord>() { new SeriesRecord() { SeriesUID = "1" }, new SeriesRecord() { SeriesUID = "2" } } };
        }
    }
}
