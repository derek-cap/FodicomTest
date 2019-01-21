using DomainModel.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Infrastructure
{
    public class MongoStudyRepository_back 
    {
        readonly static string connUrl = "mongodb://derek:123456@127.0.0.1:27017/imagedb";
        readonly static string dbName = "imagedb";
        readonly static string collectionName = "dbo.studies";

        public IQueryable<DicomStudy> AllStudies()
        {
            var collection = GetDbCollection();
            return collection.AsQueryable();
        }

        public SeriesRecord FindSeries(string seriesUID)
        {
            var collection = GetDbCollection();

            FilterDefinition<DicomStudy> filter = Builders<DicomStudy>.Filter.Where(s => s.SeriesCollection.Any(series => series.SeriesUID ==seriesUID));
            var reslut = collection.Find(filter).ToList();
            if (reslut.FirstOrDefault() != null)
            {
                return reslut.FirstOrDefault().SeriesCollection.FirstOrDefault(s => s.SeriesUID == seriesUID);
            }
            return null;
        }

        public async Task AddOrUpdateSeriesAsync(DicomStudy study)
        {
            await InsertStudyToDbAsync(study);
        }

        static async Task InsertStudyToDbAsync(DicomStudy study)
        {
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            var collection = db.GetCollection<DicomStudy>(collectionName);
            var findOnes = collection.Find(s => s.StudyUID == study.StudyUID);
            if (findOnes.Any())
            {
                var one = findOnes.First();
                foreach (var item in study.SeriesCollection)
                {
                    if (one.SeriesCollection.Any(s => s.SeriesUID == item.SeriesUID) == false)
                    {
             //           one.SeriesCollection.Add(item);
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
        /// Add or update an image record (contains in <see cref="DicomStudy"/>) to database.
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        public async Task AddOrUpdateImageAsync(DicomStudy study)
        {
            // Check image record.
            if (CheckImageLevel(study) == false) return;

            var collection = GetDbCollection();
            await InsertImageAsync(study, collection);
        }

        private async Task InsertImageAsync(DicomStudy study, IMongoCollection<DicomStudy> collection)
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
                var addSeries = Builders<DicomStudy>.Update.Push(s => s.SeriesCollection, study.SeriesCollection.FirstOrDefault());
                await collection.UpdateOneAsync(s => s.StudyUID == study.StudyUID, addSeries);
                return;
            }

            // Has image?
            //var theImage = study.SeriesCollection.First().ImageCollection.FirstOrDefault();
            //var image = series.ImageCollection.FirstOrDefault(img => img.SOPInstanceUID == theImage.SOPInstanceUID);           
            //if (image == null)
            //{
            //    var filter = Builders<DicomStudy>.Filter.Where(s => s.StudyUID == study.StudyUID)
            //        & Builders<DicomStudy>.Filter.Where(d => d.SeriesCollection.Any(r => r.SeriesUID == series.SeriesUID));
            //    var updateSeries = Builders<DicomStudy>.Update.Push(s => s.SeriesCollection[-1].ImageCollection, theImage);
            //    await collection.UpdateOneAsync(filter, updateSeries);
            //    return;
            //}
            //// Need change image record?
            //if (image.Equals(theImage) == false)
            //{
            //    image.ImageNumber = theImage.ImageNumber;
            //    image.ReferencedFileID = theImage.ReferencedFileID;
            //    var imageFilter = Builders<DicomStudy>.Filter.Where(s => s.StudyUID == study.StudyUID)
            //            & Builders<DicomStudy>.Filter.Where(d => d.SeriesCollection.Any(r => r.SeriesUID == series.SeriesUID));
            //    var updateImage = Builders<DicomStudy>.Update.Set(ss => ss.SeriesCollection[-1].ImageCollection, series.ImageCollection);
            //    await collection.UpdateOneAsync(imageFilter, updateImage);
            //}
        }

        private bool CheckImageLevel(DicomStudy study)
        {
            var theImage = study.SeriesCollection.FirstOrDefault()?.ImageCollection.FirstOrDefault();
            return theImage != null;
        }

        private IMongoCollection<DicomStudy> GetDbCollection()
        {
            var client = new MongoClient(connUrl);
            IMongoDatabase db = client.GetDatabase(dbName);

            return db.GetCollection<DicomStudy>(collectionName);
        }
    }
}
