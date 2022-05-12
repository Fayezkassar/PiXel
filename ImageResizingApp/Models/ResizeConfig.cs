using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models
{
    public class ResizeConfig
    {

        public int progressPercentage { get; set; }
        public int successNumber { get; set; }

        public decimal totalCount { get; set; }

        public double spaceGain { get; set; }

        public ResizeConfig(int v1, int v2, decimal v3, double v4)
        {
            this.progressPercentage = v1;
            this.successNumber = v2;
            this.totalCount = v3;
            this.spaceGain = v4;
        }

        public ResizeConfig()
        {
            
        }

        public class ProgressChangedEventHandler : EventArgs
        {
            public ResizeConfig Config
            {
                get;
                private set;
            }

            public ProgressChangedEventHandler(ResizeConfig config)
            {
                Config = config;
            }
        }
    }
}
