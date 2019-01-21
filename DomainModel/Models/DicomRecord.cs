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

        #region Write record to dataset
        public static void WriteStudy(this DicomDataset dataset, DicomStudy study)
        {
            dataset.AddOrUpdate(DicomTag.PatientName, study.Paitent.PatientName);
            dataset.AddOrUpdate(DicomTag.PatientID, study.Paitent.PatientID);
            dataset.AddOrUpdate(DicomTag.PatientSex, study.Paitent.PatientSex);

            dataset.AddOrUpdate(DicomTag.StudyInstanceUID, study.StudyUID);
            dataset.AddOrUpdate(DicomTag.StudyID, study.StudyID);
            dataset.AddOrUpdate(DicomTag.StudyDate, study.StudyDate);
            dataset.AddOrUpdate(DicomTag.StudyTime, study.StudyTime);

            dataset.AddOrUpdate(DicomTag.NumberOfStudyRelatedInstances, study.SeriesCollection.Count.ToString());
        }

        public static void WriteSeries(this DicomDataset dataset, string studyUID, SeriesRecord series)
        {
            dataset.AddOrUpdate(DicomTag.StudyInstanceUID, studyUID);

            dataset.AddOrUpdate(DicomTag.SeriesInstanceUID, series.SeriesUID);
            dataset.AddOrUpdate(DicomTag.SeriesNumber, series.SeriesNumber);
            dataset.AddOrUpdate(DicomTag.SeriesDate, series.SeriesDate);
            dataset.AddOrUpdate(DicomTag.SeriesTime, series.SeriesTime);
            dataset.AddOrUpdate(DicomTag.Modality, series.Modality);

            dataset.AddOrUpdate(DicomTag.NumberOfSeriesRelatedInstances, series.ImageCollection.Count.ToString());
        }

        #endregion
    }
}
