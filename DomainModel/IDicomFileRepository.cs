using Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public interface IDicomFileRepository
    {
        Task<string> SaveAsync(DicomDataset dicom);

        Task<DicomDataset> OpenAsync(string fileName);
    }
}
