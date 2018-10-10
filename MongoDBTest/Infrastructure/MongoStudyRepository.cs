using MongoDB.Driver;
using MongoDBTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTest.Infrastructure
{
    public class MongoStudyRepository : IStudyRepository
    {
        readonly static string connUrl = "mongodb://127.0.0.1:27017";
        readonly static string dbName = "imagedb";
        readonly static string collectionName = "dbo.studies";

        public async Task AddOrUpdateAsync(StudyRecord study)
        {
            await InsertStudyToDbAsync(study);
        }

        static async Task InsertStudyToDbAsync(StudyRecord study)
        {
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            var collection = db.GetCollection<StudyRecord>(collectionName);
            var findOnes = collection.Find(s => s.StudyUID == study.StudyUID);
            if (findOnes.Any())
            {
                var one = findOnes.First();
                foreach (var item in study.SeriesCollection)
                {
                    if (one.SeriesCollection.Any(s => s.SeriesUID == item.SeriesUID) == false)
                    {
                        one.SeriesCollection.Add(item);
                    }
                }
                await collection.ReplaceOneAsync(s => s.StudyUID == study.StudyUID, one);
            }
            else
            {
                await collection.InsertOneAsync(study);
            }
        }
    }
}
