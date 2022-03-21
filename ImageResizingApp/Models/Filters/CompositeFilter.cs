﻿using ImageMagick;
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

        private IEnumerable<IFilter> _filters;
        public void Process(MagickImage image)
        {
            foreach(IFilter filter in _filters)
            {
                filter.Process(image);
            }
        }
        public IFilter Clone()
        {
            return (IFilter)MemberwiseClone();
        }
    }
}
