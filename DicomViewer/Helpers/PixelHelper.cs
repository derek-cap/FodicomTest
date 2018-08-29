using Dicom.Imaging;
using Dicom.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer.Helpers
{
    class PixelHelper
    {
        #region Smooth and Sharper

        /// <summary>
        /// Smooth the <see cref="PinnedIntArray"/> with mask (1,1,1, 1,1,1, 1,1,1).
        /// </summary>
        /// <param name="width">Pixel width</param>
        /// <param name="height">Pixel height</param>
        /// <param name="pixels">Pixel data which will be changed.</param>
        public static void Smooth(int width, int height, PinnedIntArray pixels)
        {
            int[] original = new int[width * height];
            // ARGB -> B as the PinnedIntArray is color32.
            Parallel.For(0, height, h =>
            {
                var index = h * width;
                for (int i = 0; i < width; i++)
                {
                    original[index + i] = new Color32(pixels.Data[index + i]).B;
                }
            });
            // Smooth the gray pixel.
            int[] smooth = Smooth(width, height, original);
            // Gray -> ARGB as the PinnedIntArray is color32.
            Parallel.For(0, height, h =>
            {
                var index = h * width;
                for (int i = 0; i < width; i++)
                {
                    var b = new Color32(smooth[index + i]).B;
                    pixels.Data[index + i] = new Color32(0xff, b, b, b).Value;
                }
            });
        }

        /// <summary>
        /// Sharp the <see cref="PinnedIntArray"/> with mask (-1,-1,-1, -1,9,-1, -1,-1,-1). 
        /// </summary>
        /// <param name="width">Pixel width</param>
        /// <param name="height">Pixel height</param>
        /// <param name="pixels">Pixel data which will be changed.</param>
        public static void Sharp(int width, int height, PinnedIntArray pixels)
        {
            int[] original = new int[width * height];
            // ARGB -> B as the PinnedIntArray is color32.
            Parallel.For(0, height, h =>
            {
                var index = h * width;
                for (int i = 0; i < width; i++)
                {
                    original[index + i] = new Color32(pixels.Data[index + i]).B;
                }
            });
            // Sharp the gray pixel.
            int[] smooth = Sharp(width, height, original);
            // Gray -> ARGB as the PinnedIntArray is color32.
            Parallel.For(0, height, h =>
            {
                var index = h * width;
                for (int i = 0; i < width; i++)
                {
                    var b = new Color32(smooth[index + i]).B;
                    pixels.Data[index + i] = new Color32(0xff, b, b, b).Value;
                }
            });
        }

        // Smooth the int array.
        public static int[] Smooth(int width, int height, int[] original)
        {
            int[] result = new int[width * height];
            original.CopyTo(result, 0);
            int[] mean = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            for (int row = 1; row < height - 1; row++)
            {
                for (int col = 1; col < width - 1; col++)
                {
                    int index = 0, temp = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int c = original[(row + i) * width + col + j]; ;
                            temp += c * mean[index++];
                        }
                    }
                    result[row * width + col] = temp / 9;
                }
            }
            return result;
        }

        // Sharper the int array.
        private static int[] Sharp(int width, int height, int[] original)
        {
            int[] result = new int[width * height];
            original.CopyTo(result, 0);
            int[] mean = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };

            for (int row = 1; row < height - 1; row++)
            {
                for (int col = 1; col < width - 1; col++)
                {
                    int index = 0, temp = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int c = original[(row + i) * width + col + j]; ;
                            temp += c * mean[index++];
                        }
                    }
                    result[row * width + col] = temp;
                }
            }
            return result;
        }

        #endregion
    }
}
