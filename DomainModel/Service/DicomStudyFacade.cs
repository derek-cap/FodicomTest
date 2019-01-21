using DomainModel.Infrastructure;
using DomainModel.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Service
{
    public class DicomStudyFacade : StudiesContext
    {
        public IMongoQueryable<DicomStudy> AllStuies => Studies.AsQueryable();

        public DicomStudyFacade()
            : base(DbSettings.StudySettings)
        {

        }
    }
}
