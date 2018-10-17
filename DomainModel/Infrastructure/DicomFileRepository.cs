using Dicom;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Infrastructure
{
    public class DicomFileRepository : IDicomFileRepository
    {
        private const string DIRECTORY = "C:\\MongoDicom\\";

        public async Task<DicomDataset> OpenAsync(string fileName)
        {
            // Add root directory.
            string fullName = Path.Combine(DIRECTORY, fileName);
            // Load file.
            var dicomFile = await DicomFile.OpenAsync(fullName);
            return dicomFile.Dataset;
        }

        public async Task<string> SaveAsync(DicomDataset dicom)
        {
            // Generate file name.
            string studyUID = dicom.GetSingleValue<string>(DicomTag.StudyInstanceUID);
            string seriesUID = dicom.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
            string imageUID = dicom.GetSingleValue<string>(DicomTag.SOPInstanceUID);
            string filename = GenerateFilename(studyUID, seriesUID, imageUID);

            // Create directory.
            FileInfo info = new FileInfo(filename);
            Directory.CreateDirectory(info.Directory.FullName);

            // Save file
            var dicomFile = new DicomFile(dicom);
            await dicomFile.SaveAsync(filename);
            return filename.Replace(DIRECTORY, "");
        }

        // Generate full file name with path.
        private string GenerateFilename(string studyUID, string seriesUID, string imageUID)
        {
            return Path.Combine(DIRECTORY, studyUID, seriesUID, imageUID + ".dcm");
        }
    }
}
