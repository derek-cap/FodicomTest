using Dicom;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public interface IImporter
    {
        Task ImportAsync(string filePath);

        Task ImportAsync(DicomDataset dicom);       
    }
}
