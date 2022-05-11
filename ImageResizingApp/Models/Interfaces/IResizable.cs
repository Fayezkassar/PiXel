using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IResizable
    {
        public bool CheckQuality();
        public bool Resize();
        public void BackUp();
    }
}
