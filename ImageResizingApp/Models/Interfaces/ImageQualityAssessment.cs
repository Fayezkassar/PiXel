using ImageMagick;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Interfaces
{
    interface ImageQualityAssessment
    {
        public bool Compare(MagickImage originalImage, MagickImage resultingImage);
    }
}
