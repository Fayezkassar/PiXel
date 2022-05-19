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
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public int Counter { get; set; }
        public int SuccessNumber { get; set; }
        public decimal SpaceGain { get; set; }
        public decimal TotalCount { get; set; }

        private List<IFilter> _filters = new List<IFilter>();
        public void Process(MagickImage image)
        {
            foreach(IFilter filter in _filters)
            {
                filter.Process(image);
                //image.Write("C:\\Users\\Paola\\Desktop\\" + filter.Name + ".png"); // to remove
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
