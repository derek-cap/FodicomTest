using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.Communication
{
    public class QueryConditionModel : BindableBase
    {
        private string _patientName;
        private string _patientID;
        private string _patientSex;

        private string _studyID;
        private string _studyDate;
        private string _modality;
        private string _studyDescription;

        private string _referringPhysician;
        private string _accessionNumber;

        public string PatientName
        {
            get { return _patientName; }
            set { SetProperty(ref _patientName, value); }
        }

        public string PatientID
        {
            get { return _patientID; }
            set { SetProperty(ref _patientID, value); }
        }

        public string PatientSex
        {
            get { return _patientSex; }
            set { SetProperty(ref _patientSex, value); }
        }

        public string StudyID
        {
            get { return _studyID; }
            set { SetProperty(ref _studyID, value); }
        }

        public string StudyDate
        {
            get { return _studyDate; }
            set { SetProperty(ref _studyDate, value); }
        }

        public string Modality
        {
            get { return _modality; }
            set { SetProperty(ref _modality, value); }
        }

        public string StudyDescription
        {
            get { return _studyDescription; }
            set { SetProperty(ref _studyDescription, value); }
        }

        public string ReferringPhysician
        {
            get { return _referringPhysician; }
            set { SetProperty(ref _referringPhysician, value); }
        }

        public string AccessionNumber
        {
            get { return _accessionNumber; }
            set { SetProperty(ref _accessionNumber, value); }
        }

        public QueryConditionModel()
        {
            _patientName = "";
            _patientID = "";
            _patientSex = "Any";

            _studyID = "";
            _studyDate = "";
            _modality = "";
            _studyDescription = "";

            _referringPhysician = "";
            _accessionNumber = "";
        }

        /// <summary>
        /// Reset the properties to default value.
        /// </summary>
        public void Reset()
        {
            _patientName = "";
            _patientID = "";
            _patientSex = "Any";

            _studyID = "";
            _studyDate = "";
            _modality = "";
            _studyDescription = "";

            _referringPhysician = "";
            _accessionNumber = "";
        }
    }
}
