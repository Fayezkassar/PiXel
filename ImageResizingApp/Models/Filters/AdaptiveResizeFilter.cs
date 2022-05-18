using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageResizingApp.Models.Filters
{
    public class AdaptiveResizeFilter : IFilter
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public int Counter { get; set; }
        public int SuccessNumber { get; set; }
        public decimal SpaceGain { get; set; }
        public decimal TotalCount { get; set; }

        public AdaptiveResizeFilter(string name)
        {
            Name = name;
        }
        public void Process(MagickImage image)
        {
            image.AdaptiveResize(Width, Height);
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}
