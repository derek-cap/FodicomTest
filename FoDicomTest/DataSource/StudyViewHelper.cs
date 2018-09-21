using Dicom;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.DataSource
{
    public class StudyViewHelper : BindableBase
    {
        #region StudyRecondProperty
        public string AccessionNumber { get; set; }
        public string StudyInstanceUID { get; set; }
        public string StudyID { get; set; }
        public string PatientName { get; set; }
        public string PatientIndex { get; set; }
        public string PatientID { get; set; }
        public string PatientAge { get; set; }
        public string StudyDate { get; set; }            //检查日期
        //public string Sex { get; set; }
        //public string Age { get; set; }
        public string StudyTime { get; set; }            //检查时间
        public string Modality { get; set; }                //一个检查中含有的不同检查类型
        public string StudyDescription { get; set; }
        public string BodyPartExamined { get; set; }
        public string ProtocolName { get; set; }
        public string referringPhysician { get; set; }
        public string OperatorName { get; set; }

        private string _lockedState;
        public string LockedState
        {
            get { return _lockedState; }
            set { SetProperty(ref _lockedState, value); }
        }

        public bool IsSearched { get; set; }
        public bool IsCompleted { get; set; }
        public string IsFilmed { get; set; }
        public string NumberOfStudyRelatedInstances { get; set; }
        public string NumberOfStudyRelatedSeries { get; set; }
        public bool transfered { get; set; }

        #endregion

        public bool IsLocked => LockedState == "1";


        #region AddProperty

        private string age;
        public string Age
        {
            get { return age; }
            set { age = value; }
        }

        private string sex;
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        #endregion

        /// <summary>
        /// Lock this study view.
        /// </summary>
        public void Lock()
        {
            LockedState = "1";
        }

        /// <summary>
        /// Unlock this study view.
        /// </summary>
        public void Unlock()
        {
            LockedState = "0";
        }

        public static StudyViewHelper CreateFrom(DicomDataset dataset)
        {
            StudyViewHelper stuTemp = new StudyViewHelper();
            stuTemp.AccessionNumber = dataset.Get<string>(DicomTag.AccessionNumber, null);
            stuTemp.Age = dataset.Get<string>(DicomTag.PatientAge, null);
            stuTemp.BodyPartExamined = dataset.Get<string>(DicomTag.PatientAge, null);
            stuTemp.StudyDate = dataset.Get<string>(DicomTag.StudyDate, null);
            stuTemp.StudyDescription = dataset.Get<string>(DicomTag.StudyDescription, null);
            stuTemp.StudyID = dataset.Get<string>(DicomTag.StudyID, null);
            stuTemp.StudyInstanceUID = dataset.Get<string>(DicomTag.StudyInstanceUID, null);
            stuTemp.StudyTime = dataset.Get<string>(DicomTag.StudyTime, null);
            stuTemp.PatientID = dataset.Get<string>(DicomTag.PatientID, null);
            stuTemp.referringPhysician = dataset.Get<string>(DicomTag.ReferringPhysicianName, null);
            stuTemp.OperatorName = dataset.Get<string>(DicomTag.OperatorsName, null);
            stuTemp.Modality = dataset.Get<string>(DicomTag.Modality, null);
            stuTemp._lockedState = "1";
            stuTemp.IsFilmed = "false";
            stuTemp.ProtocolName = dataset.Get<string>(DicomTag.ProtocolName, null);
            stuTemp.PatientName = dataset.Get<string>(DicomTag.PatientName, null);
            stuTemp.Sex = dataset.Get<string>(DicomTag.PatientSex, null);

            return stuTemp;
        }
    }
}
