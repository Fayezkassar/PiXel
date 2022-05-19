using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImageResizingApp.Models.QualityAssessment
{
    public class HotelDieuIQA : IQualityAssessment
    {
        public bool Compare(MagickImage originalImage, MagickImage resultingImage)
        {
            double originalSize = originalImage.Width * originalImage.Height * originalImage.BitDepth();
            double resultingSize = resultingImage.Width * resultingImage.Height * resultingImage.BitDepth();
            if (resultingSize< 300000 * 8)
            {
                return false;
            }
            if (resultingSize < originalSize * 0.03)
            {
                return false;
            }
            return true;
        }
    }
}
