using Dicom.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer.Models
{
    public class PixelData
    {
        private int _width;
        private int _height;

        private int _bpp;
        private byte[] _data;

        public PixelData(int wdth, int hgt, int numOfByte)
        {
            _width = wdth;
            _height = hgt;
            _bpp = numOfByte;
        }

        public int Width => _width;

        public int Height => _height;

        public int BPP => _bpp;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

    }
}
