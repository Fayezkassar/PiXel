using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImageResizingApp.Models.QualityAssessment
{
    public class HotelDieuIQA: IQualityAssessment
    {
        public double[] Compare(MagickImage originalImage, MagickImage resultingImage)
        {
            if (resultingImage.Width * resultingImage.Height * resultingImage.Depth < 300000 * 8)
            {
                double[] b = new double[4];
                return b;
            }
            MagickImage resultingGreyscaleImage = new MagickImage(resultingImage);
            resultingGreyscaleImage.Grayscale();
            MagickImage originalGreyScaleImage = new MagickImage(originalImage);
            originalGreyScaleImage.Grayscale();

            MagickImage resultingImageForGradient = new MagickImage(resultingImage);
            MagickImage resultingImageForEdgeScore = new MagickImage(resultingImage);

            double edgeBasedScore = GetEdgeScore(originalImage, resultingImageForEdgeScore);

            double rmsContrast = GetRmsContrastScore(originalGreyScaleImage, resultingGreyscaleImage);

            double gradientSharpness = GetGradient(resultingImageForGradient);

            double score = gradientSharpness * rmsContrast * Math.Pow(edgeBasedScore, 4.0);

            //if ((resultingImage.BaseWidth * resultingImage.BaseHeight)* resultingImage.BitDepth()  > 100000)
            //{
            //    return true;

            //}
            double[] a = new double[4];
                a[0] = edgeBasedScore;
            a[1] = rmsContrast;
            a[2] = gradientSharpness;
            a[3] = score;
            return a;
        }

        private double GetEdgeScore(MagickImage originalImage, MagickImage resultingImage)
        {
            originalImage.CannyEdge();
            resultingImage.CannyEdge();

            var pixels = originalImage.GetPixels().GetValues();
            var pixels2 = resultingImage.GetPixels().GetValues();

            double totalOriginalEdgePixels = originalImage.GetPixels().GetValues().Count(x => x !=0 );
            double totalResultingEdgePixels = resultingImage.GetPixels().GetValues().Count(x => x !=0 );

            return Math.Abs(totalOriginalEdgePixels - totalResultingEdgePixels) / totalOriginalEdgePixels;
        }

        private double GetRmsContrastScore(MagickImage originalImage, MagickImage resultingImage)
        {
            double rmsContrastResulting = resultingImage.Statistics().Composite().StandardDeviation / Quantum.Max * 1.0;
            double rmsContrastOriginal = originalImage.Statistics().Composite().StandardDeviation / Quantum.Max * 1.0;
            double a = Math.Abs(rmsContrastOriginal - rmsContrastResulting) / rmsContrastOriginal;
            return rmsContrastResulting;
        }

        private double GetGradient(MagickImage image)
        {
            image.Statistic(StatisticType.Gradient, 10, 10);
            return image.Statistics().Composite().Mean;
        }
    }
}
