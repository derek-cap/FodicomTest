﻿using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using DomainModel.Service;

namespace DomainModel.Infrastructure
{
    public class DicomImageFinderService : IDicomImageFinderService
    {
        private readonly DicomStudyFacade _context;

        public DicomImageFinderService()
        {
            _context = new DicomStudyFacade();
        }

        public List<string> FindFilesByPatient(string patientId)
        {
            var studies = from s in _context.AllStuies
                          where s.Paitent.PatientID == patientId
                          select s;

            List<ImageRecord> images = new List<ImageRecord>();
            foreach (var study in studies)
            {
                foreach (var series in study.SeriesCollection)
                {
                    images.AddRange(series.ImageCollection);
                }
            }
            return (from i in images select i.ReferencedFileID).ToList();
        }

        public List<string> FindFilesByStudyUID(string studyUID)
        {
            var study = _context.AllStuies.Where(s => s.StudyUID == studyUID).FirstOrDefault();
            List<ImageRecord> images = new List<ImageRecord>();
            if (study != null)
            {
                foreach (var series in study.SeriesCollection)
                {
                    images.AddRange(series.ImageCollection);
                }
            }
            return (from i in images select i.ReferencedFileID).ToList();
        }

        public List<string> FindFilesBySeriesUID(string seriesUID)
        {
            var result = _context.AllStuies
                .SelectMany(study => study.SeriesCollection)
                .Where(s => s.SeriesUID == seriesUID)
                .FirstOrDefault();

            List<ImageRecord> images = new List<ImageRecord>();
            if (result != null)
            {
                images.AddRange(result.ImageCollection);
            }
            return (from i in images select i.ReferencedFileID).ToList();
        }
        

        public List<string> FindPatientFiles(string patientName, string patientId)
        {
            throw new NotImplementedException();
        }

        public List<string> FindSeriesFiles(string patientName, string patientId, string accessionNbr, string studyUID, string seriesUID, string modality)
        {
            throw new NotImplementedException();
        }

        public List<string> FindStudyFiles(string patientName, string patientId, string accessionNbr, string studyUID)
        {
            throw new NotImplementedException();
        }

        private bool MatchFilter(string filterValue, string valueToTest)
        {
            if (string.IsNullOrEmpty(filterValue))
            {
                // If the QR SCU sends an empty tag, then no filtering should happen
                return true;
            }
            // Take into account, that strings may contain a *-wildcard.
            var filterRegex = "^" + Regex.Escape(filterValue).Replace("\\*", ".*") + "$";
            return Regex.IsMatch(valueToTest, filterRegex, RegexOptions.IgnoreCase);
        }
    }
}
