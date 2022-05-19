using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Linq;
using ImageMagick;

namespace ImageResizingApp.Helpers
{
    public class Utilities
    {
        public static string GetFormatedSize(double value, int decimalPlaces = 1)
        {
            string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int i = 0;
            while (Math.Round(value, decimalPlaces) >= 1024)
            {
                value /= 1024;
                i++;
            }
            return string.Format("{0:n" + decimalPlaces + "} {1}", value, SizeSuffixes[i]);
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public static string GeneratePrimaryKeyValuePairsString(IEnumerable<string> primaryKeys, IEnumerable<string> values)
        {
            string finalPks = "";
            int j = 0;
            foreach (string key in primaryKeys)
            {
                if (j != 0) finalPks += " AND ";
                finalPks += key + "=" + values.ElementAt(j);
                ++j;
            }
            return finalPks;
        }

        public static double GetSpaceGain(MagickImage originalImg, MagickImage img)
        {
            return ((originalImg.Width * originalImg.Height * originalImg.BitDepth()) - (img.Width * img.Height * img.BitDepth())) / 12;
        }
    }
}
