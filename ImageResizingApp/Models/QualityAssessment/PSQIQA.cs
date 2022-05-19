using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImageResizingApp.Models.QualityAssessment
{
    public class PSQIQA : IQualityAssessment
    {
        public bool Compare(MagickImage originalImage, MagickImage resultingImage)
        {

            MagickImage resultingGImageForRmsContrast = new MagickImage(resultingImage);
            MagickImage resultingImageForGradient = new MagickImage(resultingImage);
            MagickImage resultingImageForEdgeScore = new MagickImage(resultingImage);

            double edgeBasedScore = GetEdgeScore(new MagickImage(originalImage), resultingImageForEdgeScore);

            double rmsContrast = GetRmsContrastScore(resultingGImageForRmsContrast);

            double gradientSharpness = GetGradient(resultingImageForGradient);

            double score = rmsContrast * gradientSharpness * Math.Pow(edgeBasedScore, 4.0);


            return score < 8000;
        }

        private double GetEdgeScore(MagickImage originalImage, MagickImage resultingImage)
        {
            originalImage.CannyEdge();
            resultingImage.CannyEdge();

            var pixels = originalImage.GetPixels().GetValues();
            var pixels2 = resultingImage.GetPixels().GetValues();

            double totalOriginalEdgePixels = originalImage.GetPixels().GetValues().Count(x => x != 0);
            double totalResultingEdgePixels = resultingImage.GetPixels().GetValues().Count(x => x != 0);

            return Math.Abs(totalOriginalEdgePixels - totalResultingEdgePixels) / totalOriginalEdgePixels;
        }

        private double GetRmsContrastScore(MagickImage resultingImage)
        {
            resultingImage.Grayscale();
            return resultingImage.Statistics().Composite().StandardDeviation / Quantum.Max * 1.0;
        }

        private double GetGradient(MagickImage resultingImage)
        {
            resultingImage.Grayscale();
            resultingImage.Statistic(StatisticType.Gradient, 4, 4);
            return resultingImage.Statistics().Composite().Maximum;
        }
    }

}
