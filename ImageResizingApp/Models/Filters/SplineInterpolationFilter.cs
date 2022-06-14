using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageResizingApp.Models.Filters
{
    public class SplineInterpolationFilter : IFilter
    {
        public Dictionary<string, int> Parameters { get; set; }
        public string Name { get; set; }
        public SplineInterpolationFilter(string name)
        {
            Name = name;
            Parameters = new Dictionary<string, int>();
            Parameters.Add("Width", 0);
            Parameters.Add("Height", 0);
        }
        public void Process(MagickImage image)
        {
            int width, height;
            if (Parameters.TryGetValue("Width", out width) && Parameters.TryGetValue("Height", out height)){
                image.InterpolativeResize(width, height, PixelInterpolateMethod.Spline);
            }
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}
