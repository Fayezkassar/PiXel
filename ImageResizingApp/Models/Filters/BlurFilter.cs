using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Filters
{
    public class BlurFilter : IFilter
    {
        public Dictionary<string, int> Parameters { get; set; }
        public string Name { get; set; }
        public BlurFilter(string name)
        {
            Name = name;
        }
        public void Process(MagickImage image)
        {
            image.Blur();
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}