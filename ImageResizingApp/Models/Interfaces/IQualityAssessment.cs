using ImageMagick;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IQualityAssessment
    {
        public double[] Compare(MagickImage originalImage, MagickImage resultingImage);
    }
}
