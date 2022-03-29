using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IFilter
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public void Process(MagickImage image);
        public IFilter Clone();
    }
}
