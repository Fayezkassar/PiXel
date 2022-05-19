using ImageMagick;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IQualityAssessment
    {
        public bool Compare(MagickImage originalImage, MagickImage resultingImage);
    }
}
