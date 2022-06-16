using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Filters
{
    public class ReduceNoiseFilter : IFilter
    {
        public Dictionary<string, int> Parameters { get; set; }
        public string Name { get; set; }
        public ReduceNoiseFilter(string name)
        {
            Name = name;
            Parameters = new Dictionary<string, int>();
            Parameters.Add("Order", 0);
        }
        public void Process(MagickImage image)
        {
            int order;
            if (Parameters.TryGetValue("Order", out order))
            {
                image.ReduceNoise(order);
            }
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}