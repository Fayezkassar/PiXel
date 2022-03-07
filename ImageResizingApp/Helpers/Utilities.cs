using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
