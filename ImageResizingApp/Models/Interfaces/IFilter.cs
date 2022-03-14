using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IFilter
    {
        public Image Process(Image image);
    }
}
