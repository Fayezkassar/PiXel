using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageResizingApp.Models.Filters
{
    public class LiquidRescaleFilter : IFilter
    {
        public Dictionary<string, int> Parameters { get; set; }
        public string Name { get; set; }
        public LiquidRescaleFilter(string name)
        {
            Name = name;
            Parameters = new Dictionary<string, int>();
            Parameters.Add("Width", 0);
            Parameters.Add("Height", 0);
        }
        public void Process(MagickImage image)
        {
            int width, height;
            if (Parameters.TryGetValue("Width", out width) && Parameters.TryGetValue("Height", out height))
            {
                image.LiquidRescale(width, height);

            }
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}
