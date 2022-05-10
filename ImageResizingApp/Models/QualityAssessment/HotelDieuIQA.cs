using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.QualityAssessment
{
    public class HotelDieuIQA: ImageQualityAssessment
    {
        public bool Compare(MagickImage originalImage, MagickImage resultingImage)
        {
            MagickImage orig2 = new MagickImage("C:\\Users\\Paola\\Desktop\\2817292.jpg");
            MagickImage res2 = new MagickImage("C:\\Users\\Paola\\Desktop\\2817292-resized.jpg");
            MagickImage orig3 = new MagickImage("C:\\Users\\Paola\\Desktop\\png.png");
            MagickImage res3 = new MagickImage("C:\\Users\\Paola\\Desktop\\png-resized.png");
            MagickImage orig1 = new MagickImage("C:\\Users\\Paola\\Desktop\\792735.png");
            MagickImage res1 = new MagickImage("C:\\Users\\Paola\\Desktop\\792735-resized.png");
            orig1.CannyEdge();
            res1.CannyEdge();
            orig2.CannyEdge();
            res2.CannyEdge();
            orig3.CannyEdge();
            res3.CannyEdge();

            orig1.Write("C:\\Users\\Paola\\Desktop\\2817292-canny.png");
            res1.Write("C:\\Users\\Paola\\Desktop\\2817292-resized-canny.png");
            orig3.Write("C:\\Users\\Paola\\Desktop\\png-canny.png");
            res3.Write("C:\\Users\\Paola\\Desktop\\png-resized-canny.png");
            orig1.Write("C:\\Users\\Paola\\Desktop\\792735-canny.png");
            res2.Write("C:\\Users\\Paola\\Desktop\\792735-resized-canny.png");

            if ((resultingImage.BaseWidth * resultingImage.BaseHeight)* resultingImage.BitDepth()  > 100000)
            {
                return true;
            }
            return true;
        }
    }
}
