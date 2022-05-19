﻿using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageResizingApp.Models.Filters
{
    public class SplineInterpolationFilter : IFilter
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public SplineInterpolationFilter(string name)
        {
            Name = name;
        }
        public void Process(MagickImage image)
        {
            image.InterpolativeResize(Width, Height, PixelInterpolateMethod.Spline);
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}
