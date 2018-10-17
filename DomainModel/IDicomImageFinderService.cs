﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public interface IDicomImageFinderService
    {
        List<string> FindPatientFiles(string patientName, string patientId);

        List<string> FindStudyFiles(string patientName, string patientId, string accessionNbr, string studyUID);

        List<string> FindSeriesFiles(string patientName, string patientId, string accessionNbr, string studyUID, string seriesUID, string modality);

        List<string> FindFilesByPatient(string patientId);

        List<string> FindFilesByStudyUID(string studyUID);

        List<string> FindFilesBySeriesUID(string seriesUID);
    }
}
