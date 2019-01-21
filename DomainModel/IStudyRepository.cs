using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public interface IStudyRepository
    {
        Task<DicomStudy> Add(DicomStudy study);

        Task Update(DicomStudy study);

        Task<DicomStudy> GetAsync(string studyUID);
    }
}
