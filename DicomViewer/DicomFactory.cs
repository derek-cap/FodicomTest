using Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer
{
    class DicomFactory
    {
        public static DicomDataset CreateDicom()
        {
            string filename = @"D:\Dicom\ws_2.dcm";
            DicomFile file = DicomFile.Open(filename);
            return file.Dataset;
        }
    }
}
