using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer.Models
{
    public class DcmImage
    {
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }

        public int PixelCount
        {
            get { return PixelWidth * PixelHeight; }
        }

        public ushort BitsAllocated { get; set; }
        public ushort BitsStored { get; set; }
        public ushort HighBit { get; set; }

        public ushort PixelRepresentation { get; set; }
        public ushort SamplesPerPixel { get; set; }

        public string PhotometricInterpretation { get; set; }

        public byte[] PixelData { get; set; }

        /// <summary>
        /// M (HU = M*SV + B)
        /// </summary>
        public double RescaleSlope { get; set; } = 1;
        /// <summary>
        /// B (HU = M*SV + B)
        /// </summary>
        public double RescaleIntercept { get; set; } = 0;

        public UInt16[] GetUInt16PixelData(double windowCenter, double windowWidth)
        {
            // Get the window border to special the image data.
            int windowLowBorder = (windowWidth <= 0) ? 20 : (int)(windowCenter - windowWidth / 2);
            int windowHighBorder = (windowWidth <= 0) ? 100 : (int)(windowCenter + windowWidth / 2);

            UInt16[] buffer = new UInt16[PixelCount];

            switch (this.BitsAllocated)
            {
                case 16:
                    Parallel.For(0, PixelCount,
                        i =>
                        {
                            Int16 value = BitConverter.ToInt16(PixelData, 2 * i);
                            buffer[i] = MapToUInt16(value, windowLowBorder, windowHighBorder, (int)windowWidth);
                        });
                    break;
                case 8:
                    Parallel.For(0, PixelCount,
                        i =>
                        {
                            sbyte value = Convert.ToSByte(PixelData[i]);
                            buffer[i] = MapToUInt16(value, windowLowBorder, windowHighBorder, (int)windowWidth);
                        });
                    break;
                default:
                    break;
            }

            return buffer;
        }

        public Int16[] GetColorImagePixelData()
        {
            Int16[] rawPixelData = new Int16[PixelData.Count()];
            switch (BitsAllocated)
            {
                case 16:
                    Parallel.For(0, PixelData.Count(),
                        i =>
                        {
                            Int16 value = BitConverter.ToInt16(PixelData, 2 * i);
                            rawPixelData[i] = (Int16)(value * RescaleSlope + RescaleIntercept);
                        });
                    break;
                case 8:
                    Parallel.For(0, PixelData.Count(),
                        i =>
                        {
                            byte value = PixelData[i];
                            rawPixelData[i] = (Int16)(value * RescaleSlope + RescaleIntercept);
                        });
                    break;
                default:
                    break;
            }

            return rawPixelData;
        }

        #region Priavet method
        private UInt16 MapToUInt16(Int16 value, int lowBorder, int highBorder, int windowWidth)
        {
            // Convert the value to Hounsfield (HU = M*SV + B).
            value = (Int16)(value * RescaleSlope + RescaleIntercept);
            UInt16 rValue = 0;
            // pb(x, y) = (p(x,y) - LowBorder) / WindowWidth * Imax
            if (value <= lowBorder)
            {
                rValue = 0;
            }
            else if (value <= highBorder)
            {
                rValue = (UInt16)(UInt16.MaxValue * (value - lowBorder) / windowWidth);
            }
            else
            {
                rValue = UInt16.MaxValue;
            }
            return rValue;
        }

        private UInt16 MapToUInt16(sbyte value, int lowBorder, int highBorder, int windowWidth)
        {
            // Convert the value to Hounsfield (HU = M*SV + B).
            value = (sbyte)(value * RescaleSlope + RescaleIntercept);
            UInt16 rValue = 0;
            // pb(x, y) = (p(x,y) - LowBorder) / WindowWidth * Imax
            if (value <= lowBorder)
            {
                rValue = 0;
            }
            else if (value <= highBorder)
            {
                rValue = (UInt16)(sbyte.MaxValue * (value - lowBorder) / windowWidth);
            }
            else
            {
                rValue = (UInt16)sbyte.MaxValue;
            }
            return rValue;
        }
        #endregion
    }
}
