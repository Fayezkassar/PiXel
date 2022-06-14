using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace ImageResizingApp.Models.Filters
{
    public class CompositeFilter : IFilter
    {
        public Dictionary<string, int> Parameters { get; set; }
        public string Name { get; set; }

        private List<IFilter> _filters = new List<IFilter>();
        public void Process(MagickImage image)
        {
            foreach(IFilter filter in _filters)
            {
                filter.Process(image);
            }
        }

        public void AddFilter(IFilter filter)
        {
            _filters.Add(filter);
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}
