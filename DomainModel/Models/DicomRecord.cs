using Dicom;

namespace DomainModel.Models
{
    public static class DicomRecord
    {
        #region To data base record
        public static PatientRecord ToPatient(DicomDataset record)
        {
            PatientRecord patient = new PatientRecord();
            patient.PatientName = record.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
            patient.PatientID = record.GetSingleValueOrDefault<string>(DicomTag.PatientID, null);
            patient.PatientBirthDate = record.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
            patient.PatientSex = record.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
            return patient;
        }

        public static StudyRecord ToStudy(DicomDataset record)
        {
            StudyRecord study = new StudyRecord();
            study.StudyUID = record.GetSingleValue<string>(DicomTag.StudyInstanceUID);
            study.StudyID = record.GetSingleValueOrDefault<string>(DicomTag.StudyID, null);
            study.StudyDate = record.GetSingleValueOrDefault<string>(DicomTag.StudyDate, null);
            study.StudyTime = record.GetSingleValueOrDefault<string>(DicomTag.StudyTime, null);
            return study;
        }

        public static SeriesRecord ToSeries(DicomDataset record)
        {
            SeriesRecord series = new SeriesRecord();
            series.SeriesUID = record.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
            series.SeriesNumber = record.GetSingleValueOrDefault<string>(DicomTag.SeriesNumber, null);
            series.SeriesDate = record.GetSingleValueOrDefault<string>(DicomTag.SeriesDate, null);
            series.SeriesTime = record.GetSingleValueOrDefault<string>(DicomTag.SeriesTime, null);
            series.Modality = record.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
            return series;
        }

        public static ImageRecord ToImage(DicomDataset record)
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
