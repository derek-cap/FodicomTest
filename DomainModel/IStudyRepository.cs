using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public interface IStudyRepository
    {
        Task AddOrUpdateSeriesAsync(StudyRecord study);

        /// <summary>
        /// Add or update an image record (contains in <see cref="StudyRecord"/>) to database.
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        Task AddOrUpdateImageAsync(StudyRecord study);

        IQueryable<StudyRecord> AllStudies();

        SeriesRecord FindSeries(string seriesUID);
    }
}
