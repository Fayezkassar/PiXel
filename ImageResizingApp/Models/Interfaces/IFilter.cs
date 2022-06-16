using ImageMagick;
using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.Interfaces
{
    /*! IFilter Interface */
    public interface IFilter
    {
        public string Name { get; set; }
        public Dictionary<string,int> Parameters { get; set; }
        public void Process(MagickImage image);
        public IFilter Clone();
    }
}
