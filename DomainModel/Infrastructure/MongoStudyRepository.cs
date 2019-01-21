using DomainModel.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Infrastructure
{
    public class MongoStudyRepository : IStudyRepository
    {
        private readonly StudiesContext _context;

        public MongoStudyRepository()
            : this(DbSettings.StudySettings)
        { }

        public MongoStudyRepository(DbSettings dbSettings)
        {
            _context = new StudiesContext(dbSettings);
        }

        public async Task<DicomStudy> Add(DicomStudy study)
        {
            var collection = _context.Studies;
            await collection.InsertOneAsync(study).ConfigureAwait(false);
            return study;
        }

        public async Task<DicomStudy> GetAsync(string studyUID)
        {
            try
            {
                var collection = _context.Studies;
                var result = await collection.FindAsync(s => s.StudyUID == studyUID).ConfigureAwait(false);
                return await result.FirstAsync();
            }
            catch (Exception)
            {
                return await Task.FromResult(NotFoundStudy.Instance);
            }         
        }

        public async Task Update(DicomStudy study)
        {
            var collection = _context.Studies;
            await collection.ReplaceOneAsync(s => s.StudyUID == study.StudyUID, study);
        }
    }
}
