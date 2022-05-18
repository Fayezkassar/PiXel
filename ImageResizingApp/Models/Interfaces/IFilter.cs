using ImageMagick;
using System;

namespace ImageResizingApp.Models.Interfaces
{
    /*! IFilter Interface */
    public interface IFilter
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public void Process(MagickImage image);
        public IFilter Clone();
        public int Counter { get; set; }
        public int SuccessNumber { get; set; }
        public decimal SpaceGain { get; set; }
        public decimal TotalCount { get; set; }
    }
}
