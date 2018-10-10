using Dicom.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dicom;

namespace MongoDBTest.Models
{
    class Importer
    {
        private IStudyRepository _repo;

        public Importer(IStudyRepository repository)
        {
            _repo = repository;
        }

        public async Task ImportAsync(string filePath)
        {
            // Collect datasets.
            var dicomDirectory = await Task.Run(() => WriteMedia(filePath));
            // Transport to records.
            var studies = ReadMedia(dicomDirectory);
            // Insert to database.
            foreach (var item in studies)
            {
                await _repo.AddOrUpdateAsync(item);
            }
        }

        /// <summary>
        /// Scan files and write to <see cref="DicomDirectory"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static DicomDirectory WriteMedia(string path)
        {
            var dirInfo = new DirectoryInfo(path);

            var dicomDir = new DicomDirectory();
            foreach (var file in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var dicomFile = DicomFile.Open(file.FullName);
                dicomDir.AddFile(dicomFile, string.Format(@"00001\{0}", file.Name));
            }
            return dicomDir;
        }

        private static List<StudyRecord> ReadMedia(DicomDirectory dicomDirectory)
        {
            List<StudyRecord> studies = new List<StudyRecord>();

            foreach (var patientRecord in dicomDirectory.RootDirectoryRecordCollection)
            {
                var patient = ToPatient(patientRecord);

                // Get studies.
                foreach (var studyRecord in patientRecord.LowerLevelDirectoryRecordCollection)
                {                    
                    var study = ToStudy(studyRecord);
                    study.Paitent = patient;

                    // Get series.
                    foreach (var seriesRecord in studyRecord.LowerLevelDirectoryRecordCollection)
                    {                        
                        var series = ToSeries(seriesRecord);

                        // Get images.
                        foreach (var imageRecord in seriesRecord.LowerLevelDirectoryRecordCollection)
                        {                            
                            var image = ToImage(imageRecord);
                            series.ImageCollection.Add(image);
                            Console.WriteLine($"Read image {image.ImageNumber}");
                        }

                        study.SeriesCollection.Add(series);
                    }

                    studies.Add(study);
                }
            }

            return studies;
        }

        private static void InsertToDatabase()
        {

        }

        #region To data base record
        private static PatientRecord ToPatient(DicomDirectoryRecord record)
        {
            PatientRecord patient = new PatientRecord();
            patient.PatientName = record.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
            patient.PatientID = record.GetSingleValueOrDefault<string>(DicomTag.PatientID, null);
            patient.PatientBirthDate = record.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
            patient.PatientSex = record.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
            return patient;
        }

        private static StudyRecord ToStudy(DicomDirectoryRecord record)
        {
            StudyRecord study = new StudyRecord();
            study.StudyUID = record.GetSingleValue<string>(DicomTag.StudyInstanceUID);
            study.StudyID = record.GetSingleValueOrDefault<string>(DicomTag.StudyID, null);
            study.StudyDate = record.GetSingleValueOrDefault<string>(DicomTag.StudyDate, null);
            study.StudyTime = record.GetSingleValueOrDefault<string>(DicomTag.StudyTime, null);
            return study;
        }

        private static SeriesRecord ToSeries(DicomDirectoryRecord record)
        {
            SeriesRecord series = new SeriesRecord();
            series.SeriesUID = record.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
            series.SeriesNumber = record.GetSingleValueOrDefault<string>(DicomTag.SeriesNumber, null);
            series.SeriesDate = record.GetSingleValueOrDefault<string>(DicomTag.SeriesDate, null);
            series.SeriesTime = record.GetSingleValueOrDefault<string>(DicomTag.SeriesTime, null);
            series.Modality = record.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
            return series;
        }

        private static ImageRecord ToImage(DicomDirectoryRecord record)
        {
            ImageRecord image = new ImageRecord();
            image.SOPInstanceUID = record.GetSingleValue<string>(DicomTag.ReferencedSOPInstanceUIDInFile);
            image.ImageNumber = record.GetSingleValueOrDefault<string>(DicomTag.InstanceNumber, null);
            image.ReferencedFileID = record.GetValueOrDefault<string>(DicomTag.ReferencedFileID, 0, null);
            return image;
        }

        #endregion
    }
}
