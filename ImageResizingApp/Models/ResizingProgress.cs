using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models
{
    public class ResizingProgress
    {

        public int ProgressPercentage => (ImageCount / TotalImageCount) * 100;
        public int SuccessCount { get; set; }
        public int TotalImageCount { get; set; }
        public int ImageCount { get; set; }
        public double SpaceGain { get; set; }

        public ResizingProgress(int imageCount, int successCount, int totalImageCount, double spaceGain)
        {
            ImageCount = imageCount;
            SuccessCount = successCount;
            TotalImageCount = totalImageCount;
            SpaceGain = spaceGain;
        }

        public ResizingProgress()
        {
            ImageCount = 0;
            SuccessCount = 0;
            TotalImageCount = 1;
            SpaceGain = 0;
        }

        public class ProgressChangedEventHandler : EventArgs
        {
            public ResizingProgress Config
            {
                get;
                private set;
            }

            public ProgressChangedEventHandler(ResizingProgress config)
            {
                Config = config;
            }
        }
    }
}
