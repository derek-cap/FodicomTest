using Dicom;
using DomainModel.Events;
using DomainModel.Infrastructure;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel.Models
{
    public class DicomStudy
    {
        [BsonId]
        public string StudyUID { get; private set; }

        [BsonElement("study_id")]
        public string StudyID { get; set; }

        [BsonElement("study_date")]
        public string StudyDate { get; set; }

        [BsonElement("study_time")]
        public string StudyTime { get; set; }

        [BsonElement("study_description")]
        public string StudyDescription { get; set; }

        [BsonElement("accession_number")]
        public string AccessionNumber { get; set; }

        [BsonElement("patient")]
        public PatientRecord Paitent { get; set; }

        [BsonElement("series")]
        private List<SeriesRecord> _seriesItems;        // Cannot be readonly as Mongodb serialize problem

        public IReadOnlyCollection<SeriesRecord> SeriesCollection => _seriesItems;

        [BsonElement("study_status")]
        public int StudyStatus { get; set; }

        protected DicomStudy()
        {
            _seriesItems = new List<SeriesRecord>();
        }

        protected DicomStudy(string studyUID, string studyId, string studyDate, string studyTime, string accessinoNumber, PatientRecord patient, string description)
        {
            _seriesItems = new List<SeriesRecord>();
            StudyUID = studyUID;
            StudyID = studyId;
            StudyDate = studyDate;
            StudyTime = studyTime;
            AccessionNumber = accessinoNumber;
            Paitent = patient;
            StudyDescription = description;
        }

        public static DicomStudy CreateFromDicom(DicomDataset dicom)
        {
            string studyUID = dicom.GetSingleValue<string>(DicomTag.StudyInstanceUID);
            string studyId = dicom.GetSingleValueOrDefault<string>(DicomTag.StudyID, null);
            string studyDate = dicom.GetSingleValueOrDefault<string>(DicomTag.StudyDate, null);
            string studyTime = dicom.GetSingleValueOrDefault<string>(DicomTag.StudyTime, null);
            string accessionNumber = dicom.GetSingleValueOrDefault<string>(DicomTag.AccessionNumber, null);
            string description = dicom.GetSingleValueOrDefault<string>(DicomTag.StudyDescription, null);

            PatientRecord patient = DicomRecord.ToPatient(dicom);
            var result = new DicomStudy(studyUID, studyId, studyDate, studyTime, accessionNumber, patient, description);

            SeriesRecord series = DicomRecord.ToSeries(dicom);
            ImageRecord image = new ImageRecord();
            image.SOPInstanceUID = dicom.GetSingleValue<string>(DicomTag.SOPInstanceUID);
            image.ImageNumber = dicom.GetSingleValueOrDefault<string>(DicomTag.InstanceNumber, null);
            series.AddImage(image);
            result._seriesItems.Add(series);

            // Notice the event.
            AddNewStudyEvent(result);

            return result;
        }

      
        public ImageRecord AddImageItem(DicomDataset dicom)
        {
            string seriesUID = dicom.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
            var series = SeriesCollection.FirstOrDefault(s => s.SeriesUID == seriesUID);
            if (series == null)
            {
                series = DicomRecord.ToSeries(dicom);
                _seriesItems.Add(series);
                // Add new series.
                AddNewSeriesEvent(this);
            }

            string imageUID = dicom.GetSingleValue<string>(DicomTag.SOPInstanceUID);
            ImageRecord image = series.ImageCollection.FirstOrDefault(i => i.SOPInstanceUID == imageUID);
            if (image == null)
            {
                image = new ImageRecord();
                image.SOPInstanceUID = dicom.GetSingleValue<string>(DicomTag.SOPInstanceUID);
                image.ImageNumber = dicom.GetSingleValueOrDefault<string>(DicomTag.InstanceNumber, null);
                // Add new image.
                series.AddImage(image);
                AddNewImageEvent(this);
            }
            else
            {
                
            }
            return image;
        }

        private static void AddNewStudyEvent(DicomStudy result)
        {
            // This is only for test.
            StudyCreatedEventHandler handler = new StudyCreatedEventHandler(new MongoStudyRepository(DbSettings.StudySettings));
            handler.Handle(new StudyCreatedEvent() { Study = result });
        }


        private void AddNewImageEvent(DicomStudy study)
        {
            StudyUpdatedEventHandler handler = new StudyUpdatedEventHandler(new MongoStudyRepository(DbSettings.StudySettings));
            handler.Handle(new StudyUpdatedEvent() { Study = study });
        }

        private void AddNewSeriesEvent(DicomStudy study)
        {
            StudyUpdatedEventHandler handler = new StudyUpdatedEventHandler(new MongoStudyRepository(DbSettings.StudySettings));
            handler.Handle(new StudyUpdatedEvent() { Study = study });
        }
    }
}
