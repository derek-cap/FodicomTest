using MongoDBTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //     var records = RecordFactory.CreateImageRecords();
                //     InsertImageRecordToDb(records);
                var records = RecordFactory.CreateStudies();
                InsertStudyToDb(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static string connUrl = "mongodb://127.0.0.1:27017";
        static string dbName = "imagedb";

        static void InsertImageRecordToDb(IEnumerable<ImageRecord> records)
        {
            var mongo = new MongoUrl(connUrl);
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            var collection = db.GetCollection<ImageRecord>("dbo.images");
            collection.InsertMany(records);
        }

        static void DispalyImageRecords()
        {
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            var collection = db.GetCollection<ImageRecord>("dbo.images");
            foreach (var item in collection.AsQueryable())
            {
                Console.WriteLine(item.SOPInstanceUID);
            }
        }

        static void InsertStudyToDb(IEnumerable<StudyRecord> records)
        {
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            var collection = db.GetCollection<StudyRecord>("dbo.studies");
            collection.InsertMany(records);
        }
    }
}
