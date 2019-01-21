using DomainModel.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Infrastructure
{
    public class StudiesContext
    {
        private readonly IMongoDatabase _databsae = null;

        public StudiesContext(DbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _databsae = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<DicomStudy> Studies => _databsae.GetCollection<DicomStudy>("dbo.studies");
    }
}
