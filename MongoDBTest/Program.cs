using MongoDBTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.ComponentModel;
using MongoDBTest.Infrastructure;

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
                Importer importer = new Importer(new MongoStudyRepository());
                string path = @"C:\DCMFolder\imagedb\1.2.86.76547135.7.210278.20170306131520";
                importer.ImportAsync(path).Wait();
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

      

        static void UpdateStudy()
        {
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            var collection = db.GetCollection<StudyRecord>("dbo.studies");

            var filter = Builders<StudyRecord>.Filter.Where(s => s.StudyUID == "2")
                & Builders<StudyRecord>.Filter.Where(d => d.SeriesCollection.Any(s => s.SeriesUID == "1"));
            var result = collection.Find(filter).ToList();

            //var update = Builders<StudyRecord>.Update.Set(d => d.Series[-1].ImageCount, 8);
            //collection.UpdateOne(filter, update);
        }
    }
}
