using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Filters
{
    public class GreyscaleFilter: IFilter
    {
        public Dictionary<string, int> Parameters { get; set; }
        public string Name { get; set; }
        public GreyscaleFilter(string name)
        {
            Name = name;
        }
        public void Process(MagickImage image)
        {
            image.Grayscale();
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}
