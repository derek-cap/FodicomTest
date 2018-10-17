using DomainModel.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Infrastructure
{
    public class MongoStudyRepository : IStudyRepository
    {
        readonly static string connUrl = "mongodb://127.0.0.1:27017";
        readonly static string dbName = "imagedb";
        readonly static string collectionName = "dbo.studies";

        public IQueryable<StudyRecord> AllStudies()
        {
            var collection = GetDbCollection();
            return collection.AsQueryable();
        }

        public SeriesRecord FindSeries(string seriesUID)
        {
            var collection = GetDbCollection();

            FilterDefinition<StudyRecord> filter = Builders<StudyRecord>.Filter.Where(s => s.SeriesCollection.Any(series => series.SeriesUID ==seriesUID));
            var reslut = collection.Find(filter).ToList();
            if (reslut.FirstOrDefault() != null)
            {
                return reslut.FirstOrDefault().SeriesCollection.FirstOrDefault(s => s.SeriesUID == seriesUID);
            }
            return null;
        }

        public async Task AddOrUpdateSeriesAsync(StudyRecord study)
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

        /// <summary>
        /// Add or update an image record (contains in <see cref="StudyRecord"/>) to database.
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        public async Task AddOrUpdateImageAsync(StudyRecord study)
        {
            // Check image record.
            if (CheckImageLevel(study) == false) return;

            var collection = GetDbCollection();
            await InsertImageAsync(study, collection);
        }

        private async Task InsertImageAsync(StudyRecord study, IMongoCollection<StudyRecord> collection)
        {
            // Has study?
            var findOnes = collection.Find(s => s.StudyUID == study.StudyUID);            
            if (findOnes.Any() == false)
            {
                await collection.InsertOneAsync(study);
                return;
            }

            // Has series?
            var one = findOnes.First();
            var series = one.SeriesCollection.FirstOrDefault(s => s.SeriesUID == study.SeriesCollection.First().SeriesUID);            
            if (series == null)
            {
                var addSeries = Builders<StudyRecord>.Update.Push(s => s.SeriesCollection, study.SeriesCollection.FirstOrDefault());
                await collection.UpdateOneAsync(s => s.StudyUID == study.StudyUID, addSeries);
                return;
            }

            // Has image?
            var theImage = study.SeriesCollection.First().ImageCollection.FirstOrDefault();
            var image = series.ImageCollection.FirstOrDefault(img => img.SOPInstanceUID == theImage.SOPInstanceUID);           
            if (image == null)
            {
                var filter = Builders<StudyRecord>.Filter.Where(s => s.StudyUID == study.StudyUID)
                    & Builders<StudyRecord>.Filter.Where(d => d.SeriesCollection.Any(r => r.SeriesUID == series.SeriesUID));
                var updateSeries = Builders<StudyRecord>.Update.Push(s => s.SeriesCollection[-1].ImageCollection, theImage);
                await collection.UpdateOneAsync(filter, updateSeries);
                return;
            }
            // Need change image record?
            if (image.Equals(theImage) == false)
            {
                image.ImageNumber = theImage.ImageNumber;
                image.ReferencedFileID = theImage.ReferencedFileID;
                var imageFilter = Builders<StudyRecord>.Filter.Where(s => s.StudyUID == study.StudyUID)
                        & Builders<StudyRecord>.Filter.Where(d => d.SeriesCollection.Any(r => r.SeriesUID == series.SeriesUID));
                var updateImage = Builders<StudyRecord>.Update.Set(ss => ss.SeriesCollection[-1].ImageCollection, series.ImageCollection);
                await collection.UpdateOneAsync(imageFilter, updateImage);
            }
        }

        private bool CheckImageLevel(StudyRecord study)
        {
            var theImage = study.SeriesCollection.FirstOrDefault()?.ImageCollection.FirstOrDefault();
            return theImage != null;
        }

        private IMongoCollection<StudyRecord> GetDbCollection()
        {
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            return db.GetCollection<StudyRecord>(collectionName);
        }
    }
}
