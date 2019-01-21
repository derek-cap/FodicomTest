using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.ComponentModel;
using DomainModel.Infrastructure;
using NLog.Config;
using NLog.Targets;
using NLog;
using NLog.Layouts;

namespace MongoDBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BsonTest.Test();
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

            var collection = db.GetCollection<DicomStudy>("dbo.studies");

            var filter = Builders<DicomStudy>.Filter.Where(s => s.StudyUID == "2")
                & Builders<DicomStudy>.Filter.Where(d => d.SeriesCollection.Any(s => s.SeriesUID == "1"));
            var result = collection.Find(filter).ToList();

            //var update = Builders<DicomStudy>.Update.Unset(ds => ds.)
            //collection.UpdateOne(filter, update);
        }

        private static void TestLog()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);

            var fileTarget = new FileTarget("target2")
            {
                FileName = @"C:\Log\${logger}/file.txt",
                Layout = "${longdate} ${level} ${message} ${exception}"
            };
            config.AddTarget(fileTarget);

            config.AddRuleForAllLevels(consoleTarget);
            config.AddRuleForOneLevel(LogLevel.Error, fileTarget);

            LogManager.Configuration = config;

            Logger logger2 = LogManager.GetLogger("Example2");
            //     var records = RecordFactory.CreateImageRecords();
            //     InsertImageRecordToDb(records);
            //Importer importer = new Importer(new MongoStudyRepository());
            //string path = @"C:\DCMFolder\imagedb\1.2.86.76547135.7.210278.20170306131520";
            //importer.ImportAsync(path).Wait();

            //var _studyRepo = new MongoStudyRepository();
            //string seriesUID = "2.16.840.1.114492.4530665600623454105.206257310733.43174.24";

            //var series = _studyRepo.FindSeries(seriesUID);

            //Console.WriteLine(series.ImageCollection.Count);
            //Console.WriteLine("3&#\x00B2()");


            //      config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);


            Logger logger = LogManager.GetLogger("Example");
            logger.Trace("trace log message");
            logger.Debug("debug log message");
            logger.Info("info log message");
            logger.Warn("warn log message");
            logger.Error("error log message");
            logger.Fatal("fatal log message");


            logger2.Trace("trace log message");
            logger2.Debug("debug log message");
            logger2.Info("info log message");
            logger2.Warn("warn log message");
            logger2.Error("error log message");
            logger2.Fatal("fatal log message");

            throw new Exception("test exeption");
        }   
   
    }
}
