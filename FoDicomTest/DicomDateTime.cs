using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest
{
    public class DicomDateTime
    {
        private DateTime? _dateTime;

        /// <summary>
        /// Date of Dicom format
        /// </summary>
        public string DicomDateString => ToDateString(_dateTime);

        /// <summary>
        /// Time of Dicom format
        /// </summary>
        public string DicomTimeString => ToTimeString(_dateTime);

        /// <summary>
        /// Date time 
        /// </summary>
        public DateTime? StandardDateTime => _dateTime;

        public DicomDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public DicomDateTime(string dicomDateString, string dicomTimeString)
        {
            _dateTime = ToDateTime(dicomDateString, dicomTimeString);
        }

        private string ToDateString(DateTime? dateTime)
        {
            return dateTime?.ToString("yyyyMMdd");
        }

        private string ToTimeString(DateTime? dateTime)
        {
            return dateTime?.ToString("HHmmss.FFF");
        }

        private DateTime? ToDateTime(string dateString, string timeString)
        {
            DateTime theDate = new DateTime();
            if (DateTime.TryParseExact(dateString, "yyyyMMdd", null, DateTimeStyles.None, out theDate) == false)
            {
                return null;
            }
            DateTime theTime = new DateTime();
            if (DateTime.TryParseExact(timeString, "HHmmss.FFFFFF", null, DateTimeStyles.None, out theTime))
            {
                return new DateTime(theDate.Year, theDate.Month, theDate.Day, theTime.Hour, theTime.Minute, theTime.Second, theTime.Millisecond);
            }
            else
            {
                return new DateTime(theDate.Year, theDate.Month, theDate.Day);
            }
        }
    }
}
