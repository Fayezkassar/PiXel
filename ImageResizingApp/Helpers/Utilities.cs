using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.Helpers
{
    public class Utilities
    {
        public static string GetFormatedSize(decimal value, int decimalPlaces = 1)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int i = 0;
            while (Math.Round(value, decimalPlaces) >= 1000)
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

        public static string GeneratePrimaryKeyValuePairs(IEnumerable<string> primaryKeys, List<string> values)
        {
            string finalPks = "";
            int j = 0;
            foreach (string key in primaryKeys)
            {
                if (j != 0) finalPks += " AND ";
                finalPks += key + "=" + values[j];
                ++j;
            }
            return finalPks;
        }
    }
}
